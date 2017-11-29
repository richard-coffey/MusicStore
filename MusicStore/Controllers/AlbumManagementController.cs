using System.Collections.Generic;
using System.Linq;
using MusicStore.Models;
using MusicStore.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MusicStore.Controllers
{
    [Authorize(Roles = "Admin")]
    [Authorize(Policy = "DeleteAlbum")]
    public class AlbumManagementController : Controller
    {
        private readonly IAlbumRepository _AlbumRepository;
        private readonly ICategoryRepository _categoryRepository;

        public AlbumManagementController(IAlbumRepository AlbumRepository, ICategoryRepository categoryRepository)
        {
            _AlbumRepository = AlbumRepository;
            _categoryRepository = categoryRepository;
        }

        public ViewResult Index()
        {
            var Albums = _AlbumRepository.Albums.OrderBy(p => p.AlbumId);
            return View(Albums);
        }

        public IActionResult AddAlbum()
        {
            var categories = _categoryRepository.Categories;
            var AlbumEditViewModel = new AlbumEditViewModel
            {
                Categories = categories.Select(c => new SelectListItem() { Text = c.CategoryName, Value = c.CategoryId.ToString() }).ToList(),
                CategoryId = categories.FirstOrDefault().CategoryId
            };
            return View(AlbumEditViewModel);
        }

        [HttpPost]
        public IActionResult AddAlbum(AlbumEditViewModel AlbumEditViewModel)
        {
            // Custom validation rules
            if (ModelState.GetValidationState("Album.Price") == ModelValidationState.Valid  &&
                AlbumEditViewModel.Album.Price < 0)
            {
                ModelState.AddModelError(nameof(AlbumEditViewModel.Album.Price),
                    "The price of the Album should be higher than 0.");
            }

            if (AlbumEditViewModel.Album.IsAlbumOfTheWeek && !AlbumEditViewModel.Album.InStock)
            {
                ModelState.AddModelError(nameof(AlbumEditViewModel.Album.IsAlbumOfTheWeek),
                    "Only Albums that are in stock should be Album of the Week.");
            }

            AlbumEditViewModel.Album.CategoryId = AlbumEditViewModel.CategoryId;

            //Basic validation
            if (ModelState.IsValid)
            {
                _AlbumRepository.CreateAlbum(AlbumEditViewModel.Album);
                return RedirectToAction("Index");
            }
            return View(AlbumEditViewModel);
        }


        //public IActionResult EditAlbum([FromRoute]int AlbumId)
        //public IActionResult EditAlbum([FromQuery]int AlbumId, [FromHeader] string accept)
        public IActionResult EditAlbum([FromQuery]int AlbumId, [FromHeader(Name = "Accept-Language")] string accept)
        {
            var categories = _categoryRepository.Categories;

            var Album = _AlbumRepository.Albums.FirstOrDefault(p => p.AlbumId == AlbumId);

            var AlbumEditViewModel = new AlbumEditViewModel
            {
                Categories = categories.Select(c => new SelectListItem() { Text = c.CategoryName, Value = c.CategoryId.ToString() }).ToList(),
                Album = Album,
                CategoryId = Album.CategoryId
            };

            var item = AlbumEditViewModel.Categories.FirstOrDefault(c => c.Value == Album.CategoryId.ToString());
            item.Selected = true;

            return View(AlbumEditViewModel);
        }

        [HttpPost]
        //public IActionResult EditAlbum([Bind("Album")] AlbumEditViewModel AlbumEditViewModel)
        public IActionResult EditAlbum(AlbumEditViewModel AlbumEditViewModel)
        {
            AlbumEditViewModel.Album.CategoryId = AlbumEditViewModel.CategoryId;

            if (ModelState.IsValid)
            {
                _AlbumRepository.UpdateAlbum(AlbumEditViewModel.Album);
                return RedirectToAction("Index");
            }
            return View(AlbumEditViewModel);
        }

        [HttpPost]
        public IActionResult DeleteAlbum(string AlbumId)
        {
            return View();
        }

        public IActionResult QuickEdit()
        {
            var AlbumNames = _AlbumRepository.Albums.Select(p => p.Name).ToList();
            return View(AlbumNames);
        }

        [HttpPost]
        public IActionResult QuickEdit(List<string> AlbumNames)
        {
            //Do awesome things with the Album names here
            return View(AlbumNames);
        }

        public IActionResult BulkEditAlbums()
        {
            var AlbumNames = _AlbumRepository.Albums.ToList();
            return View(AlbumNames);
        }

        [HttpPost]
        public IActionResult BulkEditAlbums(List<Album> Albums)
        {
            //Do awesome things with the Album here
            return View(Albums);
        }
    }
}