using BookShop.DataAccess.Data;
using BookShop.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {

        public BookShopDbContext _db;
        public ICategoryRepository CategoryRepository { get; private set; }
        public UnitOfWork(BookShopDbContext db)
        {
            _db = db;
            CategoryRepository = new CategoryRepository(_db);
        }


        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
