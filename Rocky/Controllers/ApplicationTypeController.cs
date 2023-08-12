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
    public class ApplicationTypeController : Controller
    {
        private readonly IApplicationTypeRepository _appTypeRepos;

        public ApplicationTypeController(IApplicationTypeRepository appTypeRepos)
        {
            _appTypeRepos = appTypeRepos;
        }
        public IActionResult Index()
        {
            IEnumerable<ApplicationType> objList = _appTypeRepos.ApplicationType;
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
            if (ModelState.IsValid)
            {
                _appTypeRepos.ApplicationType.Add(obj);
                _appTypeRepos.SaveChanges();
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

            var obj = _appTypeRepos.ApplicationType.Find(id);

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
                _appTypeRepos.ApplicationType.Update(obj);
                _appTypeRepos.SaveChanges();
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

            var obj = _appTypeRepos.ApplicationType.Find(id);

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
            _appTypeRepos.ApplicationType.Remove(obj);
            _appTypeRepos.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
