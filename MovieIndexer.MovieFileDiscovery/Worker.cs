using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using MovieIndexer.MovieFileDiscovery.Services;

namespace MovieIndexer.MovieFileDiscovery
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IBusControl _bus;
        private readonly IInitialLoadOrchestrator _initialLoadOrchestrator;

        public Worker(ILogger<Worker> logger, 
            IBusControl bus,
            IInitialLoadOrchestrator initialLoadOrchestrator)
        {
            _logger = logger;
            _bus = bus;
            _initialLoadOrchestrator = initialLoadOrchestrator;
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            await _bus.StartAsync(cancellationToken);
            await base.StartAsync(cancellationToken);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await _bus.StopAsync(cancellationToken);
            await base.StopAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _initialLoadOrchestrator.PerformInitialDiscoveryAsync();
            
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
