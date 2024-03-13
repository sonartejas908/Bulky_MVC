using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            List<Product> objProductList = _unitOfWork.Product.GetAll().ToList();

            return View(objProductList);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Product obj)
        {
            //if(obj.Name == obj.DisplayOrder.ToString())
            //{
            //    ModelState.AddModelError("name", "The Display Order cannot exactly match the name");
            //}

            if (ModelState.IsValid)
            {
                _unitOfWork.Product.Add(obj);
                _unitOfWork.Save();

                TempData["success"] = "Cagegory Created Succcessfully.";
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Product? Productfromdb = _unitOfWork.Product.Get(u => u.Id == id);
            //Product? Productfromdb1 = _db.Categories.FirstOrDefault(u => u.Id == id);
            //Product? Productfromdb2 = _db.Categories.Where(u => u.Id == id).FirstOrDefault();

            if (Productfromdb == null)
            {
                return NotFound();
            }
            return View(Productfromdb);
        }
        [HttpPost]
        public IActionResult Edit(Product obj)
        {


            if (ModelState.IsValid)
            {
                _unitOfWork.Product.update(obj);
                _unitOfWork.Save();
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
            Product? Productfromdb = _unitOfWork.Product.Get(u => u.Id == id);
            //Product? Productfromdb1 = _db.Categories.FirstOrDefault(u => u.Id == id);
            //Product? Productfromdb2 = _db.Categories.Where(u => u.Id == id).FirstOrDefault();

            if (Productfromdb == null)
            {
                return NotFound();
            }
            return View(Productfromdb);
        }


        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? Id)
        {
            Product? obj = _unitOfWork.Product.Get(u => u.Id == Id);

            if (obj == null)
            {
                return NotFound();
            }
            _unitOfWork.Product.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = "Cagegory Deleted Succcessfully.";
            return RedirectToAction("Index");
        }
    }
}
