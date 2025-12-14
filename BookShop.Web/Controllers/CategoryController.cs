using BookShop.Web.Data;
using BookShop.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Web.Controllers
{
    public class CategoryController : Controller
    {
        private readonly BookShopDbContext _db;

        public CategoryController(BookShopDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            List<Category> objCategoryList = _db.Categories.ToList();
            return View(objCategoryList);
        }

        public IActionResult Create()
        {
            return View();
        }
        public IActionResult Edit()
        {
            return View();
        }
        public IActionResult Delete()
        {
            return View();
        }
    }
}
