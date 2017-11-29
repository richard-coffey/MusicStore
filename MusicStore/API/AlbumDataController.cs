using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MusicStore.Models;
using MusicStore.ViewModels;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace BethanysAlbumShop.Controllers
{
    [Route("api/Albumdata")]
    public class AlbumDataController : Controller
    {
        private const int PAGE_SIZE = 12;

        private readonly IAlbumRepository _AlbumRepository;

        public AlbumDataController(IAlbumRepository AlbumRepository)
        {
            _AlbumRepository = AlbumRepository;
        }


        [HttpGet("{category}/{pagecounter}")]
        public IEnumerable<AlbumViewModel> LoadMoreAlbums(string category, int pagecounter)
        {
            IEnumerable<Album> dbAlbums = null;

            if (string.IsNullOrEmpty(category) || category.ToUpper() == "ALL AlbumS")
            {
                dbAlbums = _AlbumRepository.Albums;

            }
            else
            {
                dbAlbums = _AlbumRepository.Albums.Where(p => p.Category.CategoryName == category);
            }

            dbAlbums = dbAlbums.OrderBy(p => p.Name).Skip(PAGE_SIZE * pagecounter).Take(PAGE_SIZE);

            List<AlbumViewModel> Albums = new List<AlbumViewModel>();

            foreach (var dbAlbum in dbAlbums)
            {
                Albums.Add(MapDbAlbumToAlbumViewModel(dbAlbum));
            }
            return Albums;
        }


        private AlbumViewModel MapDbAlbumToAlbumViewModel(Album dbAlbum)
        {
            return new AlbumViewModel()
            {
                AlbumId = dbAlbum.AlbumId,
                Name = dbAlbum.Name,
                Price = dbAlbum.Price,
                ShortDescription = dbAlbum.ShortDescription,
                ImageThumbnailUrl = dbAlbum.ImageThumbnailUrl
            };
        }
    }
}
