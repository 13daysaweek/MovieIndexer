using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MassTransit;
using Microsoft.Extensions.Logging;
using MovieIndexer.Contracts.Commands;
using MovieIndexer.MovieFileDiscovery.Consumers;
using MovieIndexer.MovieFileDiscovery.Services;

namespace MovieIndexer.MovieFileDiscovery
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Worker>();
                    services.AddSingleton<IInitialLoadFileSearcher, InitialLoadFileSearcher>(_ => new InitialLoadFileSearcher(hostContext.Configuration[Constants.ConfigurationKeys.BaseDirectory]));
                    services.AddSingleton<IInitialLoadOrchestrator, InitialLoadOrchestrator>();
                    services.AddSingleton<IThumbnailService, ThumbnailService>(_ => new ThumbnailService(hostContext.Configuration[Constants.ConfigurationKeys.MtnExeLocation], 
                        hostContext.Configuration[Constants.ConfigurationKeys.ScratchDirectory], 
                        (ILogger<ThumbnailService>)services.BuildServiceProvider().GetService(typeof(ILogger<ThumbnailService>))));
                    
                    services.AddMassTransit(x =>
                    {
                        x.AddConsumer<InitialMovieThumbnailRequestConsumer>();
                        
                        EndpointConvention.Map<AddNewMovieToReviewQueue>(new Uri("queue:review-queue"));
                        
                        x.UsingRabbitMq((context, cfg) =>
                        {
                            cfg.Host(hostContext.Configuration[Constants.ConfigurationKeys.RabbitMqHost]);
                            
                            cfg.ReceiveEndpoint("initial-movie-thumbnail-request", e =>
                            {
                                e.ConfigureConsumer<InitialMovieThumbnailRequestConsumer>(context);
                            });
                        });
                    });
                });
    }
}
