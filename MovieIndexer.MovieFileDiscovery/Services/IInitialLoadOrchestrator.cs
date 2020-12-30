using System.Threading.Tasks;

namespace MovieIndexer.MovieFileDiscovery.Services
{
    public interface IInitialLoadOrchestrator
    {
        Task PerformInitialDiscoveryAsync();
    }
}