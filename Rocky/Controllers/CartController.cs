﻿using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rocky_Models;
using Rocky_Models.ViewModels;
using Rocky.Utility;
using Microsoft.Extensions.Primitives;
using Rocky_DataAccess.Data;
using Rocky.Interfaces;
using Rocky_DataAccess;
using Rocky_DataAccess.Repository.IRepository;

namespace Rocky.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly IProductRepository _prodRepos;
        private readonly IApplicationUserRepository _appUserRepos;
        private readonly IInquiryHeaderRepository _inquiryHeaderRepos;
        private readonly IInquiryDetailRepository _inquiryDetailRepos;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IMailSender _mailSender;
        private readonly IMailTemplater _mailTemplater;
        private readonly IMessageFormater _messageFormater;

        [BindProperty]
        public ProductUserVM ProductUserVM { get; set; }
        public CartController(IProductRepository prodRepos,
            IApplicationUserRepository appUserRepos,
            IInquiryHeaderRepository inquiryHeaderRepos,
            IInquiryDetailRepository inquiryDetailRepos,
            IWebHostEnvironment webHostEnvironment, 
            IMailSender mailSender, 
            IMailTemplater mailTemplater, 
            IMessageFormater messageFormater)
        {
            _prodRepos = prodRepos;
            _appUserRepos = appUserRepos;
            _inquiryHeaderRepos = inquiryHeaderRepos;
            _inquiryDetailRepos = inquiryDetailRepos;
            _webHostEnvironment = webHostEnvironment;
            _mailSender = mailSender;
            _mailTemplater = mailTemplater;
            _messageFormater = messageFormater;
        }
        public IActionResult Index()
        {

            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart) != null
                && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart).Count() > 0)
            {
                //session exsits
                shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart);
            }

            List<int> prodInCart = shoppingCartList.Select(i => i.ProductId).ToList();
            IEnumerable<Product> prodList = _prodRepos.GetAll(u => prodInCart.Contains(u.Id));

            return View(prodList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Index")]
        public IActionResult IndexPost()
        {

            return RedirectToAction(nameof(Summary));
        }

        public IActionResult Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            //var userId = User.FindFirstValue(ClaimTypes.Name);

            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart) != null
                && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart).Count() > 0)
            {
                //session exsits
                shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart);
            }

            List<int> prodInCart = shoppingCartList.Select(i => i.ProductId).ToList();
            IEnumerable<Product> prodList = _prodRepos.GetAll(u => prodInCart.Contains(u.Id));

            ProductUserVM = new ProductUserVM()
            {
                ApplicationUser = _appUserRepos.FirstOrDefault(u => u.Id == claim.Value),
                ProductList = prodList.ToList()
            };

            return View(ProductUserVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Summary")]
        public async Task<IActionResult> SummaryPost(ProductUserVM ProductUserVM)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var sender = new MailboxAddress("Rocky", WC.EmailAdmin);
            var receiver = new MailboxAddress(ProductUserVM.ApplicationUser.FullName, ProductUserVM.ApplicationUser.Email);

            var messageBody = _mailTemplater.GetMessageBody(ProductUserVM, _webHostEnvironment);
            var message = _messageFormater.GetMimeMessage(messageBody, sender, receiver);
            _mailSender.SendEmail(WC.EmailAdmin, message, messageBody);

            InquiryHeader inquiryHeader = new InquiryHeader() 
            { 
                ApplicationUserId = claim.Value,
                FullName = ProductUserVM.ApplicationUser.FullName,
                Email = ProductUserVM.ApplicationUser.Email,
                PhoneNumber = ProductUserVM.ApplicationUser.PhoneNumber,
                InquiryDate = DateTime.Now,
            };

            _inquiryHeaderRepos.Add(inquiryHeader);
            _inquiryHeaderRepos.Save();

            foreach (var prod in ProductUserVM.ProductList)
            {
                InquiryDetail inquiryDetail = new InquiryDetail() 
                { 
                    InquiryHeaderId = inquiryHeader.Id,
                    ProductId = prod.Id
                };
                _inquiryDetailRepos.Add(inquiryDetail);
            }
            _inquiryDetailRepos.Save();

            return RedirectToAction(nameof(InquiryConfirmation));
        }
        public IActionResult InquiryConfirmation()
        {
            HttpContext.Session.Clear();
            return View();
        }
        public IActionResult Remove(int id)
        {

            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart) != null
                && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart).Count() > 0)
            {
                //session exsits
                shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart);
            }

            shoppingCartList.Remove(shoppingCartList.FirstOrDefault(u => u.ProductId == id));
            HttpContext.Session.Set(WC.SessionCart, shoppingCartList);
            return RedirectToAction(nameof(Index));
        }

        
    }
}