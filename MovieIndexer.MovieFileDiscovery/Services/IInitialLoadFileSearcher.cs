using System.Collections.Generic;

namespace MovieIndexer.MovieFileDiscovery.Services
{
    public interface IInitialLoadFileSearcher
    {
        IEnumerable<string> FindInitialMovies();
    }
}