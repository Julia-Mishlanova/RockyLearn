using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rocky.Data;
using Rocky.Models;
using Rocky.Utility;
using System.Collections.Generic;
using System.Linq;

namespace Rocky.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _dB;
        public CartController(ApplicationDbContext dB)
        {
            _dB = dB;
        }
        public IActionResult Index()
        {
            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart) != null 
                && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart).Count() != 0)
            {
                //session exist
                shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart);
            }
            List<int> prodInCart = shoppingCartList.Select(x => x.ProductId).ToList();
            IEnumerable<Product> prodList = _dB.Product.Where(x => prodInCart.Contains(x.Id));
            return View(prodList);
        }

        public IActionResult Delete(int id)
        {
            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart) != null
                && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart).Count() != 0)
            {
                //session exist
                shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart);
            }

            shoppingCartList.Remove(shoppingCartList.FirstOrDefault(u => u.ProductId == id));
            HttpContext.Session.Set(WC.SessionCart, shoppingCartList);
            return RedirectToAction(nameof(Index));
        }
    }
}
