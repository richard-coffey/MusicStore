using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MusicStore.Models;
using MusicStore.ViewModels;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace MusicStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAlbumRepository _AlbumRepository;

        public HomeController(IAlbumRepository AlbumRepository)
        {
            _AlbumRepository = AlbumRepository;
        }

        public IActionResult Index()
        {
            var homeViewModel = new HomeViewModel
            {
                AlbumsOfTheWeek = _AlbumRepository.AlbumsOfTheWeek
            };

            return View(homeViewModel);
        }
    }
}
