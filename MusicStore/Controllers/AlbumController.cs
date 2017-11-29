using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using MusicStore.Models;
using MusicStore.ViewModels;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MusicStore.Controllers
{
    public class AlbumController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IAlbumRepository _AlbumRepository;
        private readonly IAlbumReviewRepository _AlbumReviewRepository;
        private readonly HtmlEncoder _htmlEncoder;


        public AlbumController( IAlbumRepository AlbumRepository, ICategoryRepository categoryRepository, IAlbumReviewRepository AlbumReviewRepository, HtmlEncoder htmlEncoder)
        {
            _categoryRepository = categoryRepository;
            _AlbumReviewRepository = AlbumReviewRepository;
            _htmlEncoder = htmlEncoder;
            _AlbumRepository = AlbumRepository;
        }

        
        public IActionResult Index()
        {
            return RedirectToAction("List",new {category = ""});
//            return List("");
        }

        public IActionResult List(string category)
        {
            return View(new AlbumsListViewModel
            {
                CurrentCategory = category??"All Albums"
            });
        }




        // [Route("[controller]/Details/{id}")]
        public IActionResult Details(int id)
        {
            var Album = _AlbumRepository.GetAlbumById(id);
            if (Album == null)
                return NotFound();

            return View(new AlbumDetailViewModel() { Album = Album });
        }



        // [Route("[controller]/Details/{id}")]
        [HttpPost]
        public IActionResult Details(int id, string review)
        {
            var Album = _AlbumRepository.GetAlbumById(id);
            if (Album == null)
                return NotFound();

            // Protect against XSS attacks by encoding all input
            string encodedReview = _htmlEncoder.Encode(review);

            _AlbumReviewRepository.AddAlbumReview(new AlbumReview() { Album = Album, Review = encodedReview });

            return View(new AlbumDetailViewModel() { Album = Album, Review="" });
        }

    }
}
