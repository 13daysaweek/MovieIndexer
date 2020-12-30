using System.Threading.Tasks;

namespace MoveIndexers.LocalFileProcessor
{
    public interface IInitialLoadSearcher
    {
        Task FindInitialMoviesAsync();
    }
}