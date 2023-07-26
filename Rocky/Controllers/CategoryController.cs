using Microsoft.AspNetCore.Mvc;
using Rocky.Data;
using Rocky.Models;
using System.Collections.Generic;

namespace Rocky.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _dB;

        public CategoryController (ApplicationDbContext dB)
        {
            _dB = dB;
        }
        public IActionResult Index()
        {
            IEnumerable<Category> objList = _dB.Category; 
            return View(objList);
        }

        // GET - Create
        public IActionResult Create()
        {
            return View();
        }

        // POST - Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category obj)
        {
            if (ModelState.IsValid) 
            {
                _dB.Category.Add(obj);
                _dB.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }

        // GET - Edit
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var obj = _dB.Category.Find(id);

            if (obj == null) 
            { 
                return NotFound(); 
            }
            return View(obj);
        }

        // POST - Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category obj)
        {
            if (ModelState.IsValid)
            {
                _dB.Category.Update(obj);
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

            var obj = _dB.Category.Find(id);

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
            _dB.Category.Remove(obj);
            _dB.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
