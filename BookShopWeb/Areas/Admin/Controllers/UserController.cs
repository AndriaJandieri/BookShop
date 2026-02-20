using BookShop.DataAccess.Repository.IRepository;
using BookShop.Models;
using BookShop.Models.ViewModels;
using BookShop.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class UserController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUnitOfWork _unitOfWork;

        public UserController(UserManager<IdentityUser> userManager, IUnitOfWork unitOfWork, RoleManager<IdentityRole> roleManager)
        {
            _unitOfWork = unitOfWork;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        // Fixed typo: RoleManagement
        public IActionResult RoleManagement(string userId)
        {
            var appUser = _unitOfWork.ApplicationUser.Get(u => u.Id == userId, includeProperties: "Company");

            RoleManagementVM RoleVM = new RoleManagementVM()
            {
                ApplicationUser = appUser,
                RoleList = _roleManager.Roles.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Name
                }),
                CompanyList = _unitOfWork.Company.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
            };

            RoleVM.ApplicationUser.Role = _userManager.GetRolesAsync(appUser).GetAwaiter().GetResult().FirstOrDefault();

            return View(RoleVM);
        }

        [HttpPost]
        public IActionResult RoleManagement(RoleManagementVM roleManagementVM)
        {
            var appUser = _unitOfWork.ApplicationUser.Get(u => u.Id == roleManagementVM.ApplicationUser.Id);
            var oldRole = _userManager.GetRolesAsync(appUser).GetAwaiter().GetResult().FirstOrDefault();

            if (roleManagementVM.ApplicationUser.Role != oldRole)
            {
                if (roleManagementVM.ApplicationUser.Role == SD.Role_Company)
                    appUser.CompanyId = roleManagementVM.ApplicationUser.CompanyId;

                if (oldRole == SD.Role_Company)
                    appUser.CompanyId = null;

                _unitOfWork.ApplicationUser.Update(appUser);
                _unitOfWork.Save();

                _userManager.RemoveFromRoleAsync(appUser, oldRole).GetAwaiter().GetResult();
                _userManager.AddToRoleAsync(appUser, roleManagementVM.ApplicationUser.Role).GetAwaiter().GetResult();
            }
            else if (oldRole == SD.Role_Company && appUser.CompanyId != roleManagementVM.ApplicationUser.CompanyId)
            {
                appUser.CompanyId = roleManagementVM.ApplicationUser.CompanyId;
                _unitOfWork.ApplicationUser.Update(appUser);
                _unitOfWork.Save();
            }

            return RedirectToAction("Index");
        }

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            var userList = _unitOfWork.ApplicationUser.GetAll(includeProperties: "Company").ToList();

            foreach (var user in userList)
            {
                user.Role = _userManager.GetRolesAsync(user).GetAwaiter().GetResult().FirstOrDefault();

                if (user.Company == null)
                {
                    user.Company = new Company { Name = "" };
                }
            }

            return Json(new
            {
                data = userList.Select(u => new {
                    u.Id,
                    u.Name,
                    u.Email,
                    u.PhoneNumber,
                    Company = new { u.Company.Name },
                    u.Role,
                    Status = (u.LockoutEnd != null && u.LockoutEnd > DateTime.UtcNow) ? "Locked" : "Active",
                    LockoutEnd = u.LockoutEnd
                })
            });
        }

        [HttpPost]
        public IActionResult LockUnlock([FromBody] string id)
        {
            var user = _unitOfWork.ApplicationUser.Get(u => u.Id == id);
            if (user == null)
                return Json(new { success = false, message = "Error while Locking/Unlocking" });

            if (user.LockoutEnd != null && user.LockoutEnd > DateTime.UtcNow)
                user.LockoutEnd = DateTime.UtcNow; // Unlock
            else
                user.LockoutEnd = DateTime.UtcNow.AddYears(1000); // Lock

            _unitOfWork.ApplicationUser.Update(user);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Operation Successful" });
        }

        #endregion
    }
}