using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MusicStore.Models
{
    public class AlbumRepository : IAlbumRepository
    {
        private readonly AppDbContext _appDbContext;

        public AlbumRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IEnumerable<Album> Albums
        {
            get
            {
                return _appDbContext.Albums.Include(c => c.Category);
            }
        }


        public IEnumerable<Album> AlbumsOfTheWeek
        {
            get
            {
                return Albums.Where(p => p.IsAlbumOfTheWeek);
            }
        }


        public Album GetAlbumById(int AlbumId)
        {
            return _appDbContext.Albums.Include(p => p.AlbumReviews).FirstOrDefault(p => p.AlbumId == AlbumId);
        }


        public void UpdateAlbum(Album Album)
        {
            _appDbContext.Albums.Update(Album);
            _appDbContext.SaveChanges();
        }

        public void CreateAlbum(Album Album)
        {
            _appDbContext.Albums.Add(Album);
            _appDbContext.SaveChanges();
        }
    }
}
