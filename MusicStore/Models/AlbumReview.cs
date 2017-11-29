namespace MusicStore.Models
{
    public class AlbumReview
    {
        public int AlbumReviewId { get; set; }
        public Album Album { get; set; }
        public string Review { get; set; }
    }
}
