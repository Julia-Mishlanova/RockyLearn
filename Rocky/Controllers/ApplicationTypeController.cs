using Microsoft.AspNetCore.Mvc;
using Rocky.Data;
using Rocky.Models;
using System.Collections.Generic;

namespace Rocky.Controllers
{
    public class ApplicationTypeController : Controller
    {
        private readonly ApplicationDbContext _dB;

        public ApplicationTypeController(ApplicationDbContext dB)
        {
            _dB = dB;
        }
        public IActionResult Index()
        {
            IEnumerable<ApplicationType> objList = _dB.ApplicationType;
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
        public IActionResult Create(ApplicationType obj)
        {
            _dB.ApplicationType.Add(obj);
            _dB.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET - Edit
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0) 
            { 
                return NotFound();
            }

            var obj = _dB.ApplicationType.Find(id);

            if (obj == null) 
            {
                return NotFound();
            }
            return View(obj);
        }

        // POST - Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ApplicationType obj)
        {
            if (ModelState.IsValid)
            {
                _dB.ApplicationType.Update(obj);
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

            var obj = _dB.ApplicationType.Find(id);

            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }

        // POST - Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(ApplicationType obj)
        {
            if (obj == null)
            {
                return NotFound();
            }
            _dB.ApplicationType.Remove(obj);
            _dB.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
