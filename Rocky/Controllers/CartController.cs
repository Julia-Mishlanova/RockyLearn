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

namespace Rocky.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IMailSender _mailSender;
        private readonly IMailTemplater _mailTemplater;
        private readonly IMessageFormater _messageFormater;

        [BindProperty]
        public ProductUserVM ProductUserVM { get; set; }
        public CartController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment, IMailSender mailSender, IMailTemplater mailTemplater, IMessageFormater messageFormater)
        {
            _db = db;
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
            IEnumerable<Product> prodList = _db.Product.Where(u => prodInCart.Contains(u.Id));

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
            IEnumerable<Product> prodList = _db.Product.Where(u => prodInCart.Contains(u.Id));

            ProductUserVM = new ProductUserVM()
            {
                ApplicationUser = _db.ApplicationUser.FirstOrDefault(u => u.Id == claim.Value),
                ProductList = prodList.ToList()
            };

            return View(ProductUserVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Summary")]
        public async Task<IActionResult> SummaryPost(ProductUserVM ProductUserVM)
        {
            var sender = new MailboxAddress("Rocky", WC.EmailAdmin);
            var receiver = new MailboxAddress(ProductUserVM.ApplicationUser.FullName, ProductUserVM.ApplicationUser.Email);

            var messageBody = _mailTemplater.GetMessageBody(ProductUserVM, _webHostEnvironment);
            var message = _messageFormater.GetMimeMessage(messageBody, sender, receiver);
            _mailSender.SendEmail(WC.EmailAdmin, message, messageBody);

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