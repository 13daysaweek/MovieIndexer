using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace MovieIndexer.MovieFileDiscovery.Services
{
    public class ThumbnailService : IThumbnailService
    {
        private readonly string _mtnExeLocation;
        private readonly string _scratchDirectory;
        private readonly ILogger _logger;
        
        public ThumbnailService(string mtnExeLocation, string scratchDirectory, ILogger<ThumbnailService> logger)
        {
            if (string.IsNullOrWhiteSpace(mtnExeLocation))
            {
                throw new ArgumentException(nameof(mtnExeLocation));
            }

            if (string.IsNullOrWhiteSpace(scratchDirectory))
            {
                throw new ArgumentException(nameof(scratchDirectory));
            }

            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<byte[]> CreateThumbnailAsync(string inputMovie)
        {
            if (string.IsNullOrWhiteSpace(inputMovie))
            {
                throw new ArgumentException(nameof(inputMovie));
            }

            var outputFileName = Path.Combine(_scratchDirectory,
                $"{Path.GetFileNameWithoutExtension(inputMovie)}_s.jpg");

            var arguments = $"-P -O \"{_scratchDirectory}\" \"{inputMovie}\"";

            try
            {
                using var process = new Process();
                var psi = new ProcessStartInfo
                {
                    FileName = _mtnExeLocation,
                    Arguments = "",
                    CreateNoWindow = true,
                    ErrorDialog = false
                };

                process.Start();
                process.WaitForExit();
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception caught while generating thumbnail for file {inputMovie}: {e.Message}", e);
            }

            byte[] thumbnailContent = null;
            
            if (File.Exists(outputFileName))
            {
                thumbnailContent = await File.ReadAllBytesAsync(outputFileName);

                try
                {
                    File.Delete(outputFileName);
                }
                catch (Exception e)
                {
                    _logger.LogWarning($"Unable to delete thumbnail file {outputFileName}");
                }
            }
            
            return thumbnailContent;
        }
    }
}
