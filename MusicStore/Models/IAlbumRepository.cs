using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Edm;

namespace MusicStore.Models
{
    public interface IAlbumRepository
    {
        IEnumerable<Album> Albums { get; }

        IEnumerable<Album> AlbumsOfTheWeek { get; }

        Album GetAlbumById(int AlbumId);

        void CreateAlbum(Album Album);

        void UpdateAlbum(Album Album);
    }
    
}
