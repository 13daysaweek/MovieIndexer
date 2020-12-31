using System;
using System.IO;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using MovieIndexer.Contracts.Commands;
using MovieIndexer.MovieFileDiscovery.Services;

namespace MovieIndexer.MovieFileDiscovery.Consumers
{
    public class InitialMovieThumbnailRequestConsumer : IConsumer<AddInitialMovieToThumbnailQueue>
    {
        private readonly IThumbnailService _thumbnailService;
        private readonly ILogger<InitialMovieThumbnailRequestConsumer> _logger;

        public InitialMovieThumbnailRequestConsumer(IThumbnailService thumbnailService,
            ILogger<InitialMovieThumbnailRequestConsumer> logger)
        {
            _thumbnailService = thumbnailService;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        public async Task Consume(ConsumeContext<AddInitialMovieToThumbnailQueue> context)
        {
            _logger.LogInformation($"Got message for file {context.Message.MovieFilePath}");

            var thumbnail = await _thumbnailService.CreateThumbnailAsync(context.Message.MovieFilePath);
            
            _logger.LogInformation($"Successfully created thumbnail for {context.Message.MovieFilePath}");
            
            var fileInfo = new FileInfo(context.Message.MovieFilePath);
            var message = new AddNewMovieToReviewQueue
            {
                LocalFileName = context.Message.MovieFilePath,
                FileSize = fileInfo.Length,
                ThumbnailImage = thumbnail
            };

            await context.Send(message);
        }
    }
}
