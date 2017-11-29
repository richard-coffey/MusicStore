using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MusicStore.Models;
using MusicStore.ViewModels;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MusicStore.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly IAlbumRepository _AlbumRepository;
        private readonly ShoppingCart _shoppingCart;

        public ShoppingCartController(IAlbumRepository AlbumRepository, ShoppingCart shoppingCart)
        {
            _AlbumRepository = AlbumRepository;
            _shoppingCart = shoppingCart;
        }


        public IActionResult Index()
        {
            var items = _shoppingCart.GetShoppingCartItems();
            _shoppingCart.ShoppingCartItems = items;

            var shoppingCartViewModel = new ShoppingCartViewModel
            {
                ShoppingCart = _shoppingCart,
                ShoppingCartTotal = _shoppingCart.GetShoppingCartTotal()
            };

            return View(shoppingCartViewModel);
        }

        public IActionResult AddToShoppingCart(int AlbumId)
        {
            var selectedAlbum = _AlbumRepository.Albums.FirstOrDefault(p => p.AlbumId == AlbumId);

            if (selectedAlbum != null)
            {
                _shoppingCart.AddToCart(selectedAlbum, 1);
            }
            return RedirectToAction("Index");
        }

        public IActionResult RemoveFromShoppingCart(int AlbumId)
        {
            var selectedAlbum = _AlbumRepository.Albums.FirstOrDefault(p => p.AlbumId == AlbumId);

            if (selectedAlbum != null)
            {
                _shoppingCart.RemoveFromCart(selectedAlbum);
            }
            return RedirectToAction("Index");
        }

    }
}
