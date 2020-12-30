using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;

namespace MoveIndexers.LocalFileProcessor
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private IBus _bus;
        private readonly IInitialLoadSearcher _initialLoadSearcher;

        public Worker(ILogger<Worker> logger, IBus bus, IInitialLoadSearcher initialLoadSearcher)
        {
            _bus = bus;
            _initialLoadSearcher = initialLoadSearcher;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _initialLoadSearcher.FindInitialMoviesAsync();
            
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }

    public class TestMessage
    {
        public string Message { get; set; }
    }
}
