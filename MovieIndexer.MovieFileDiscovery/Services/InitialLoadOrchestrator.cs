using System;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using MovieIndexer.Contracts.Commands;

namespace MovieIndexer.MovieFileDiscovery.Services
{
    public class InitialLoadOrchestrator : IInitialLoadOrchestrator
    {
        private readonly IBus _bus;
        private readonly IInitialLoadFileSearcher _initialLoadFileSearcher;
        private readonly ILogger<InitialLoadOrchestrator> _logger;
        
        public InitialLoadOrchestrator(IBus bus,
            IInitialLoadFileSearcher initialLoadFileSearcher,
            ILogger<InitialLoadOrchestrator> logger)
        {
            _bus = bus ?? throw new ArgumentNullException(nameof(bus));
            _initialLoadFileSearcher = initialLoadFileSearcher ?? throw new ArgumentNullException(nameof(initialLoadFileSearcher));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task PerformInitialDiscoveryAsync()
        {
            _logger.LogInformation($"Getting initial files");
            var initialFileList = _initialLoadFileSearcher.FindInitialMovies();

            var messages = initialFileList.Select(path => new AddInitialMovieToThumbnailQueue {MovieFilePath = path});
            
            foreach (var message in messages)
            {
                _logger.LogInformation($"Sending message for file {message.MovieFilePath}");
                await _bus.Publish(message);
            }
        }
    }
}
