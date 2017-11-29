using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicStore.Models
{
    public interface IAlbumReviewRepository
    {
        void AddAlbumReview(AlbumReview AlbumReview);
        IEnumerable<AlbumReview> GetReviewsForAlbum(int AlbumId);
    }
}
