using MusicStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicStore.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<Album> AlbumsOfTheWeek { get; set; }
    }
}
