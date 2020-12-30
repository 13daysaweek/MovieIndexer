using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MovieIndexer.MovieFileDiscovery.Services
{
    public class InitialLoadFileSearcher : IInitialLoadFileSearcher
    {
        private readonly string _baseDirectory;

        public InitialLoadFileSearcher(string baseDirectory)
        {
            if (string.IsNullOrWhiteSpace(baseDirectory))
            {
                throw new ArgumentException(nameof(baseDirectory));
            }

            _baseDirectory = baseDirectory;
        }
        
        public IEnumerable<string> FindInitialMovies()
        {
            var extensions = new[] { ".mpg", ".avi", ".mpeg", ".wmv", ".mp4", ".divx", ".flv"};
            var entries = Directory.GetFileSystemEntries(_baseDirectory, "*.*", SearchOption.AllDirectories)
                .Where(_ => extensions.Contains(Path.GetExtension(_).ToLower()) && !File.GetAttributes(_).HasFlag(FileAttributes.Directory));

            return entries;
        }
    }
}
