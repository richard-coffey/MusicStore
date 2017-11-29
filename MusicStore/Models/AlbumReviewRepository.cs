using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicStore.Models
{
    public class AlbumReviewRepository : IAlbumReviewRepository
    {
        private readonly AppDbContext _appDbContext;

        public AlbumReviewRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public void AddAlbumReview(AlbumReview AlbumReview)
        {
            _appDbContext.AlbumReviews.Add(AlbumReview);
            _appDbContext.SaveChanges();
        }

        public IEnumerable<AlbumReview> GetReviewsForAlbum(int AlbumId)
        {
            return _appDbContext.AlbumReviews.Where(p => p.Album.AlbumId == AlbumId);
        }
    }
}
