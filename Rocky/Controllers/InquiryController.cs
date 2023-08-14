using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rocky.Utility;
using Rocky_DataAccess;
using Rocky_DataAccess.Repository.IRepository;
using Rocky_Models;
using Rocky_Models.ViewModels;
using System.Collections.Generic;

namespace Rocky.Controllers
{
    [Authorize(Roles = WC.AdminRole)]
    public class InquiryController : Controller
    {
        private readonly IInquiryHeaderRepository _inquiryHeaderRepos;
        private readonly IInquiryDetailRepository _inquiryDetailRepos;

        [BindProperty]
        public InquiryVM InquiryVM { get; set; }
        public InquiryController(IInquiryHeaderRepository inquiryHeaderRepos, 
            IInquiryDetailRepository inquiryDetailRepos)
        {
            _inquiryHeaderRepos = inquiryHeaderRepos;
            _inquiryDetailRepos = inquiryDetailRepos;
        }

        public IActionResult Index() 
        { 
            return View(); 
        }

        public IActionResult Details(int id) 
        {
            InquiryVM inquiryVM = new InquiryVM() 
            { 
                InquiryHeader = _inquiryHeaderRepos.FirstOrDefault(u => u.Id == id),
                InquiryDetail = _inquiryDetailRepos.GetAll(u => u.InquiryHeaderId == id, includeProperties:"Product")
            };
            return View(inquiryVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Details()
        {
            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();
            InquiryVM.InquiryDetail = _inquiryDetailRepos.GetAll(u => u.InquiryHeaderId == InquiryVM.InquiryHeader.Id);
            foreach (var detail in InquiryVM.InquiryDetail)
            {
                ShoppingCart shoppingCart = new ShoppingCart()
                { 
                    ProductId = detail.ProductId 
                };
                shoppingCartList.Add(shoppingCart);
            }
            HttpContext.Session.Clear();
            HttpContext.Session.Set(WC.SessionCart, shoppingCartList);
            HttpContext.Session.Set(WC.SessionInquiryId, InquiryVM.InquiryHeader.Id);
            return RedirectToAction("Index", "Cart");
        }

        [HttpPost]
        public IActionResult Delete() 
        {
            InquiryHeader inquiryHeader = _inquiryHeaderRepos.FirstOrDefault(u => u.Id == InquiryVM.InquiryHeader.Id);
            IEnumerable<InquiryDetail> inquiryDetails = _inquiryDetailRepos.GetAll(u => u.InquiryHeaderId == InquiryVM.InquiryHeader.Id);

            _inquiryDetailRepos.RemoveRange(inquiryDetails);
            _inquiryHeaderRepos.Remove(inquiryHeader);
            _inquiryHeaderRepos.Save();

            return RedirectToAction(nameof(Index));
        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetInquiryList() 
        {
            return Json(new {data = _inquiryHeaderRepos.GetAll()});
        }
        #endregion
    }
}
