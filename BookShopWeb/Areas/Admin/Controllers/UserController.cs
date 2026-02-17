using BookShop.DataAccess.Data;
using BookShop.DataAccess.Repository.IRepository;
using BookShop.Models;
using BookShop.Models.ViewModels;
using BookShop.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BookShopWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class UserController : Controller
    {
        //private readonly IUnitOfWork _unitOfWork;
        private readonly BookShopDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;

        //public UserController(IUnitOfWork unitOfWork)
        //{
        //    _unitOfWork = unitOfWork;
        //}

        public UserController(BookShopDbContext db, UserManager<IdentityUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult RoleManagement(string userId)
        {
            var user = _db.ApplicationUsers
                .Include(u => u.Company)
                .FirstOrDefault(u => u.Id == userId);

            if (user == null)
                return NotFound();

            var userRole = _db.UserRoles
                .FirstOrDefault(u => u.UserId == userId);

            string roleName = "";

            if (userRole != null)
            {
                var role = _db.Roles
                    .FirstOrDefault(r => r.Id == userRole.RoleId);

                roleName = role?.Name ?? "";
            }

            RoleManagementVM RoleVM = new RoleManagementVM()
            {
                ApplicationUser = user,
                RoleList = _db.Roles.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Name
                }),
                CompanyList = _db.Companies.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
            };

            RoleVM.ApplicationUser.Role = roleName;

            return View(RoleVM);
        }
        [HttpPost]
        public async Task<IActionResult> RoleManagement(RoleManagementVM roleManagementVM)
        {
            var applicationUser = await _db.ApplicationUsers
                .FirstOrDefaultAsync(u => u.Id == roleManagementVM.ApplicationUser.Id);

            if (applicationUser == null)
                return NotFound();

            var roles = await _userManager.GetRolesAsync(applicationUser);
            string oldRole = roles.FirstOrDefault(); // may be null

            if (oldRole != roleManagementVM.ApplicationUser.Role)
            {
                // If user had a role before, remove it
                if (!string.IsNullOrEmpty(oldRole))
                {
                    await _userManager.RemoveFromRoleAsync(applicationUser, oldRole);
                }

                // Add new role
                await _userManager.AddToRoleAsync(applicationUser, roleManagementVM.ApplicationUser.Role);

                // Handle company logic
                if (roleManagementVM.ApplicationUser.Role == SD.Role_Company)
                {
                    applicationUser.CompanyId = roleManagementVM.ApplicationUser.CompanyId;
                }
                else
                {
                    applicationUser.CompanyId = null;
                }

                await _db.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }


        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<ApplicationUser> objUserList = _db.ApplicationUsers.Include(u => u.Company).ToList();

            var userRoles = _db.UserRoles.ToList();
            var roles = _db.Roles.ToList();

            foreach (var obj in objUserList)
            {
                var userRole = userRoles.FirstOrDefault(u => u.UserId == obj.Id);

                if (userRole != null)
                {
                    var role = roles.FirstOrDefault(r => r.Id == userRole.RoleId);
                    obj.Role = role?.Name ?? "";
                }
                else
                {
                    obj.Role = "";
                }

                obj.Status = obj.LockoutEnd != null && obj.LockoutEnd > DateTime.Now
                    ? "Locked"
                    : "Active";

                obj.Company ??= new Company { Name = "" };
            }

            return Json(new { data = objUserList });
        }

        [HttpPost]
        public IActionResult LockUnlock([FromBody] string id)
        {
            var objFromDb = _db.ApplicationUsers.FirstOrDefault(u => u.Id == id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error while Locking/Unlocking" });
            }

            //while using DateTime.Now best practice is to use DateTime.UtcNow instead

            if (objFromDb.LockoutEnd != null && objFromDb.LockoutEnd > DateTime.Now)
            {
                // user is currently locked, we will unlock them
                objFromDb.LockoutEnd = DateTime.Now;
            }
            else
            {
                // user is currently unlocked, we will lock them
                objFromDb.LockoutEnd = DateTime.Now.AddYears(100);
            }

            _db.SaveChanges();


            return Json(new { success = true, message = "Operation Successful" });
        }


        #endregion
    }
}
