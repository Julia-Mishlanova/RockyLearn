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
            _dB.Category.Add(obj);
            _dB.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
