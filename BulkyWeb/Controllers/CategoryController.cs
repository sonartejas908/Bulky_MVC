using BulkyWeb.Data;
using BulkyWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BulkyWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;
        public CategoryController(ApplicationDbContext db)
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
        [HttpPost]
        public IActionResult Create(Category obj)
        {
            //if(obj.Name == obj.DisplayOrder.ToString())
            //{
            //    ModelState.AddModelError("name", "The Display Order cannot exactly match the name");
            //}

            if (ModelState.IsValid)
            {
                _db.Categories.Add(obj);
                _db.SaveChanges();

                TempData["success"] = "Cagegory Created Succcessfully.";
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Edit(int? id)
        {
            if(id == null || id == 0)
            {
                return NotFound();
            }
            Category? categoryfromdb = _db.Categories.Find(id);
            //Category? categoryfromdb1 = _db.Categories.FirstOrDefault(u => u.Id == id);
            //Category? categoryfromdb2 = _db.Categories.Where(u => u.Id == id).FirstOrDefault();

            if (categoryfromdb == null) {
            return NotFound();
            }
            return View(categoryfromdb);
        }
        [HttpPost]
        public IActionResult Edit(Category obj)
        {
           

            if (ModelState.IsValid)
            {
                _db.Categories.Update(obj);
                _db.SaveChanges();
                TempData["success"] = "Cagegory Updated Succcessfully.";
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category? categoryfromdb = _db.Categories.Find(id);
            //Category? categoryfromdb1 = _db.Categories.FirstOrDefault(u => u.Id == id);
            //Category? categoryfromdb2 = _db.Categories.Where(u => u.Id == id).FirstOrDefault();

            if (categoryfromdb == null)
            {
                return NotFound();
            }
            return View(categoryfromdb);
        }


        [HttpPost,ActionName("Delete")]
        public IActionResult DeletePOST(int? Id)
        {
            Category? obj = _db.Categories.Find(Id);

            if(obj == null)
            {
                return NotFound();
            }
            _db.Categories.Remove(obj);
            _db.SaveChanges();
            TempData["success"] = "Cagegory Deleted Succcessfully.";
            return RedirectToAction("Index");
        }
    }
}
