namespace MovieIndexer.Contracts.Commands
{
    public class AddNewMovieToReviewQueue
    {
        public string LocalFileName { get; set; }
        
        public byte[] ThumbnailImage { get; set; }
    }
}
