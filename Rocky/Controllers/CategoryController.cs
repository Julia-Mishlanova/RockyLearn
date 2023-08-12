using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rocky_DataAccess.Data;
using Rocky_DataAccess.Repository.IRepository;
using Rocky_Models;
using System.Collections.Generic;
using System.Data;

namespace Rocky.Controllers
{
    [Authorize(Roles = WC.AdminRole)]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _catRepos;

        public CategoryController (ICategoryRepository catRepos)
        {
            _catRepos = catRepos;
        }
        public IActionResult Index()
        {
            IEnumerable<Category> objList = _catRepos.GetAll(); 
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
                _catRepos.Add(obj);
                _catRepos.Save();
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

            var obj = _catRepos.Find(id);

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
                _catRepos.Update(obj);
                _catRepos.Save();
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

            var obj = _catRepos.Find(id);

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
            _catRepos.Remove(obj);
            _catRepos.Save();
            return RedirectToAction("Index");
        }
    }
}
