using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MusicStore.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MusicStore.ViewModels
{
    public class AlbumEditViewModel
    {
        public Album Album { get; set; }
        public List<SelectListItem> Categories { get; set; }
        public int CategoryId { get; set; }
    }
}
