using BookShop.DataAccess.Data;
using BookShop.DataAccess.Repository.IRepository;
using BookShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.DataAccess.Repository
{
    public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
    {

        private readonly BookShopDbContext _db;
        public ApplicationUserRepository(BookShopDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(ApplicationUser applicationUser)
        {
            //var objFromDb = _db.ApplicationUsers.FirstOrDefault(u => u.Id == applicationUser.Id);
            //if (objFromDb != null)
            //{
            //    objFromDb.Name = applicationUser.Name;
            //    objFromDb.PhoneNumber = applicationUser.PhoneNumber;
            //    objFromDb.StreetAddress = applicationUser.StreetAddress;
            //    objFromDb.City = applicationUser.City;
            //    objFromDb.State = applicationUser.State;
            //    objFromDb.PostalCode = applicationUser.PostalCode;
            //    if (applicationUser.CompanyId != null)
            //        objFromDb.CompanyId = applicationUser.CompanyId;
            //}
            _db.ApplicationUsers.Update(applicationUser);
        }
    }
}

