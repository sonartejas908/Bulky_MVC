﻿using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            List<Product> objProductList = _unitOfWork.Product.GetAll(includeProperties:"Category").ToList();
            
            return View(objProductList);
        }

        public IActionResult Upsert(int? id)
        {
            //ViewBag.CategoryList = CategoryList;
            //ViewData["CategoryList"] = CategoryList;

            ProductVM productVM = new ProductVM()
            {
                CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                Product = new Product()
            };

            if(id == null || id == 0)
            {
                //Create
                return View(productVM);
            }
            else
            {
                //Update
                productVM.Product = _unitOfWork.Product.Get(u => u.Id == id);
                return View(productVM);
            }
            
        }
        [HttpPost]
        public IActionResult Upsert(ProductVM productVM,IFormFile? file)
        {
            //if(obj.Name == obj.DisplayOrder.ToString())
            //{
            //    ModelState.AddModelError("name", "The Display Order cannot exactly match the name");
            //}

            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;

                if(file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"images\product");

                    if (!string.IsNullOrEmpty(productVM.Product.ImageUrl))
                    {
                        //delete old image
                        var oldImagePath = Path.Combine(wwwRootPath, productVM.Product.ImageUrl.TrimStart('\\'));

                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }

                    }

                    using (var fileStream = new FileStream(Path.Combine(productPath,fileName),FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    productVM.Product.ImageUrl = @"\images\product\" + fileName;
                }

                if(productVM.Product.Id == 0)
                {
                    _unitOfWork.Product.Add(productVM.Product);
                }
                else
                {
                    _unitOfWork.Product.update(productVM.Product);

                }


                _unitOfWork.Save();

                TempData["success"] = "Cagegory Created Succcessfully.";
                return RedirectToAction("Index");
            }
            else
            {
                productVM.CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
               
                return View(productVM);
            }
            
        }

        //public IActionResult Edit(int? id)
        //{
        //    if (id == null || id == 0)
        //    {
        //        return NotFound();
        //    }
        //    Product? Productfromdb = _unitOfWork.Product.Get(u => u.Id == id);
        //    //Product? Productfromdb1 = _db.Categories.FirstOrDefault(u => u.Id == id);
        //    //Product? Productfromdb2 = _db.Categories.Where(u => u.Id == id).FirstOrDefault();

        //    if (Productfromdb == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(Productfromdb);
        //}
        //[HttpPost]
        //public IActionResult Edit(Product obj)
        //{


        //    if (ModelState.IsValid)
        //    {
        //        _unitOfWork.Product.update(obj);
        //        _unitOfWork.Save();
        //        TempData["success"] = "Cagegory Updated Succcessfully.";
        //        return RedirectToAction("Index");
        //    }
        //    return View();
        //}

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
