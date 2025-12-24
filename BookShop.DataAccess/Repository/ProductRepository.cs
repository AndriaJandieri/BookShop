using BookShop.DataAccess.Data;
using BookShop.DataAccess.Repository.IRepository;
using BookShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly BookShopDbContext _db;
        public ProductRepository(BookShopDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Product obj)
        {
            _db.Products.Update(obj);

            //var objFromDb = _db.Products.FirstOrDefault(u => u.Id == obj.Id);
            //if (objFromDb != null)
            //{
            //    objFromDb.Title = obj.Title;
            //    objFromDb.Description = obj.Description;
            //    objFromDb.ISBN = obj.ISBN;
            //    objFromDb.Author = obj.Author;
            //    objFromDb.ListPrice = obj.ListPrice;
            //    objFromDb.Price = obj.Price;
            //    objFromDb.Price50 = obj.Price50;
            //    objFromDb.Price100 = obj.Price100;
            //    objFromDb.CategoryId = obj.CategoryId;
            //    if (obj.ImageUrl != null)
            //    {
            //        objFromDb.ImageUrl = obj.ImageUrl;
            //    }
            //}
        }
    }
}
