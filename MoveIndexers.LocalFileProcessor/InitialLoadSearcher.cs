using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using MovieIndexer.Contracts.Commands;

namespace MoveIndexers.LocalFileProcessor
{
    public class InitialLoadSearcher : IInitialLoadSearcher
    {
        private readonly IBus _bus;
        
        public InitialLoadSearcher(IBus bus)
        {
            _bus = bus ?? throw new ArgumentNullException(nameof(bus));
        }

        public async Task FindInitialMoviesAsync()
        {
            var extensions = new[] { "*.mpg", "*.avi", "*.mpeg", "*.wmv", "*.mp4", "*.divx", "*.flv"};
            var entries = Directory.GetFileSystemEntries(@"D:\recovery\", "*.*", SearchOption.AllDirectories);

            var messages = entries.Select(_ => new AddInitialMovieToReviewQueue {MovieFilePath = _});

            foreach (var message in messages)
            {
                Console.WriteLine($"Publishing movie {message.MovieFilePath}");
                await _bus.Publish(message);
            }
        }
    }
}
