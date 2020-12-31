using System.Threading.Tasks;

namespace MovieIndexer.MovieFileDiscovery.Services
{
    public interface IThumbnailService
    {
        Task<byte[]> CreateThumbnailAsync(string inputMovie);
    }
}