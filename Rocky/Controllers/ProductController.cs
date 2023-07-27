using Microsoft.AspNetCore.Mvc;
using Rocky.Data;
using Rocky.Models;
using System.Collections.Generic;
using System.Linq;

namespace Rocky.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _dB;

        public ProductController(ApplicationDbContext dB)
        {
            _dB = dB;
        }
        public IActionResult Index()
        {
            IEnumerable<Product> objList = _dB.Product;

            foreach (var obj in objList)
            {
                obj.Category = _dB.Category.FirstOrDefault(u => u.Id == obj.CategoryId);
            }
            return View(objList);
        }

        // GET - Upsert
        public IActionResult Upsert(int? id)
        {
            Product product = new Product();
            if (id == null)
            {
                return View(product);
            }
            else
            {
                product = _dB.Product.Find(id);

                if (product == null) return NotFound();
                return View(product);
            }
        }

        // POST - Upsert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ApplicationType obj)
        {
            if (ModelState.IsValid)
            {
                _dB.ApplicationType.Add(obj);
                _dB.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }

        // GET - Delete
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var obj = _dB.Product.Find(id);

            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }

        // POST - Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Category obj)
        {
            if(obj == null)
            {
                return NotFound();
            }
            //_dB.Product.Remove(obj);
            _dB.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
