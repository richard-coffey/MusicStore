using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MusicStore.Utility;
using Microsoft.AspNetCore.Mvc;

namespace MusicStore.Models
{
    public class Album
    {
        public int AlbumId { get; set; }

        [Display(Name = "Album Name")]
        public string Name { get; set; }

        [Display(Name = "Artist Name")]
        public string ShortDescription { get; set; }

        [Display(Name = "Description")]
        public string LongDescription { get; set; }
        public string AllergyInformation { get; set; }
        public decimal Price { get; set; }

        [ValidUrl(ErrorMessage = "That's not a valid URL")]
        public string ImageUrl { get; set; }

        [ValidUrl(ErrorMessage = "That's not a valid URL")]
        public string ImageThumbnailUrl { get; set; }
        public bool IsAlbumOfTheWeek { get; set; }
        public bool InStock { get; set; }
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }

        public virtual List<AlbumReview> AlbumReviews { get; set; }

        //Specific for Model Binding
        public SugarLevel SugarLevel { get; set; }

    }
}
