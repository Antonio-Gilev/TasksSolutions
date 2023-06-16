using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using ProductsListing.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsListing.Controllers
{
    public class ProductController : Controller
    {
        private readonly DataAccess _dataAccess;

        public ProductController(IConfiguration configuration)
        {
            _dataAccess = new DataAccess(configuration);
        }



        public ActionResult Index()
        {
            List<Product> products = _dataAccess.GetAllProducts();

                return View(products);
        }



        public ActionResult Create()
        {
            var categories = _dataAccess.GetAllCategories();
            ViewBag.Categories = new SelectList(categories, "CategoryID", "CategoryName");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                bool access = _dataAccess.CreateProduct(product);
                if(access == true)
                {
                    return RedirectToAction("Index");
                }
            }

            var categories = _dataAccess.GetAllCategories();
            ViewBag.Categories = new SelectList(categories, "CategoryID", "CategoryName");

            return View(product);
        }

        public IActionResult Delete(int id)
        {
            bool access = _dataAccess.DeleteProduct(id);

            if(access == false)
            {
                return BadRequest();
            }

            return RedirectToAction("Index");

        } 
    }
}
