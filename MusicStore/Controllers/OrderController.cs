using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MusicStore.Auth;
using MusicStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MusicStore.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderRepository _orderRepository;

        private readonly ShoppingCart _shoppingCart;

        public OrderController(IOrderRepository orderRepository, ShoppingCart shoppingCart)
        {
            _orderRepository = orderRepository;
            _shoppingCart = shoppingCart;
        }


        public IActionResult CheckOut()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        [Authorize(Policy = "MinimumOrderAge")]
        public IActionResult CheckOut(Order order)
        {
            var items = _shoppingCart.GetShoppingCartItems();

            // not needed...  _shoppingCart.ShoppingCartItems is allways assigned in GetShoppingCartItems
            // _shoppingCart.ShoppingCartItems = items;

            if (_shoppingCart.ShoppingCartItems.Count == 0)
            {
                ModelState.AddModelError("", "Your cart is empty, add some Albums first");
            }

            if (ModelState.IsValid)
            {
                _orderRepository.CreateOrder(order);
                _shoppingCart.ClearCart();
                return RedirectToAction("CheckoutComplete");
            }
            return View(order);



        }

        public IActionResult CheckoutComplete()
        {
            if (!string.IsNullOrEmpty(HttpContext.User.Identity.Name))
            {
                ViewBag.CheckoutCompleteMessage = HttpContext.User.Identity.Name +
                                                  ", thanks for your order!";
            }
            else
            {
                ViewBag.CheckoutCompleteMessage = ", thanks for your order!";
            }
            

            return View();

        }
    }
}
