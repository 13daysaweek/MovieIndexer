using System;
using System.Collections.Generic;
using System.Text;

namespace MovieIndexer.Contracts.Commands
{
    public class AddInitialMovieToReviewQueue
    {
        public string MovieFilePath { get; set; }
    }
}
