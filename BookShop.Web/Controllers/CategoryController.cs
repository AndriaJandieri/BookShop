using BookShop.DataAccess.Data;
using BookShop.DataAccess.Repository.IRepository;
using BookShop.Models;
using Microsoft.AspNetCore.Mvc;

public class CategoryController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public CategoryController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }


    public IActionResult Index()
    {
        List<Category> objCategoryList = _unitOfWork.Category.GetAll().ToList();
        return View(objCategoryList);
    }
    #region Create
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(Category obj)
    {
        if (obj.Name == obj.Description.ToString())
        {
            ModelState.AddModelError("name", "The DisplayOrder cannot exactly match the Name.");
        }

        if (obj.Name != null && obj.Name.ToLower() == "test")
        {
            ModelState.AddModelError("", "The Name cannot be 'test'.");
        }

        if (ModelState.IsValid)
        {
            _unitOfWork.Category.Add(obj);
            _unitOfWork.Save();
            //TempData["success"] = $"Category <u><b>{obj.Name}</b></u> created successfully";
            TempData["success"] = "Category created successfully";
            return RedirectToAction("Index", "Category");
        }
        return View();

    }
    #endregion
    #region Edit
    public IActionResult Edit(int? id)
    {
        if (id == 0 || id == null)
        {
            return NotFound();
        }
        Category? categoryFromDb = _unitOfWork.Category.Get(u => u.Id == id);
        //Category? categoryFromDb2 = _db.Categories.FirstOrDefault(u => u.Id == id);
        //Category? categoryFromDb3 = _db.Categories.Where(u => u.Id == id).FirstOrDefault();


        if (categoryFromDb == null)
        {
            return NotFound();
        }

        //OldCategoryName = categoryFromDb.Name;
        return View(categoryFromDb);
    }

    [HttpPost]
    public IActionResult Edit(Category obj)
    {

        if (ModelState.IsValid)
        {
            _unitOfWork.Category.Update(obj);
            _unitOfWork.Save();
            //TempData["success"] = $"Category <u><b>{OldCategoryName}</b></u> updated to <u><b>{obj.Name}</b></u> successfully";
            TempData["success"] = "Category updated successfully";
            return RedirectToAction("Index", "Category");
        }
        return View();

    }
    #endregion
    #region Delete
    public IActionResult Delete(int? id)
    {
        if (id == 0 || id == null)
        {
            return NotFound();
        }
        Category? categoryFromDb = _unitOfWork.Category.Get(u => u.Id == id);
        //Category? categoryFromDb2 = _db.Categories.FirstOrDefault(u => u.Id == id);
        //Category? categoryFromDb3 = _db.Categories.Where(u => u.Id == id).FirstOrDefault();

        if (categoryFromDb == null)
        {
            return NotFound();
        }
        return View(categoryFromDb);
    }

    //[HttpPost, ActionName("Delete")]
    //public IActionResult DeletePOST(Category obj)
    //{
    //    _db.Categories.Remove(obj);
    //    _db.SaveChanges();
    //    return RedirectToAction("Index", "Category");

    //}

    [HttpPost, ActionName("Delete")]
    public IActionResult DeletePOST(int? id)
    {
        Category? obj = _unitOfWork.Category.Get(u => u.Id == id);
        if (obj == null)
        {
            return NotFound();
        }
        _unitOfWork.Category.Remove(obj);
        _unitOfWork.Save();
        //TempData["success"] = $"Category <u><b>{obj.Name}</b></u> deleted successfully";
        TempData["success"] = "Category deleted successfully";
        return RedirectToAction("Index", "Category");
    }
    #endregion
}