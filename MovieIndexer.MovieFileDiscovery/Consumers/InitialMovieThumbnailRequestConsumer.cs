using System;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using MovieIndexer.Contracts.Commands;

namespace MovieIndexer.MovieFileDiscovery.Consumers
{
    public class InitialMovieThumbnailRequestConsumer : IConsumer<AddInitialMovieToThumbnailQueue>
    {
        private readonly ILogger<InitialMovieThumbnailRequestConsumer> _logger;

        public InitialMovieThumbnailRequestConsumer(ILogger<InitialMovieThumbnailRequestConsumer> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        public Task Consume(ConsumeContext<AddInitialMovieToThumbnailQueue> context)
        {
            _logger.LogInformation($"Got message for file {context.Message.MovieFilePath}");
            return Task.CompletedTask;
        }
    }
}
