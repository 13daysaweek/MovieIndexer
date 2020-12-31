using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MassTransit;
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
                    services.AddSingleton<IInitialLoadFileSearcher, InitialLoadFileSearcher>(_ => new InitialLoadFileSearcher(hostContext.Configuration["baseDirectory"]));
                    services.AddSingleton<IInitialLoadOrchestrator, InitialLoadOrchestrator>();
                    
                    services.AddMassTransit(x =>
                    {
                        x.AddConsumer<InitialMovieThumbnailRequestConsumer>();
                        
                        x.UsingRabbitMq((context, cfg) =>
                        {
                            cfg.Host(hostContext.Configuration["rabbitMqHost"]);
                            
                            cfg.ReceiveEndpoint("initial-movie-thumbnail-request", e =>
                            {
                                e.ConfigureConsumer<InitialMovieThumbnailRequestConsumer>(context);
                            });
                        });
                        
                        //x.AddBus(factory => Bus.Factory.CreateUsingRabbitMq(_ =>
                        //{
                        //    _.Host(hostContext.Configuration["rabbitMqHost"]);
                            
                        //    //_.ReceiveEndpoint("initial-movie-thumbnail-requests", ep =>
                        //    //{
                        //    //    ep.Consumer<InitialMovieThumbnailRequestConsumer>();
                        //    //});
                        //}));

                        //x.AddConsumer<InitialMovieThumbnailRequestConsumer>();

                        //cfg.AddRequestClient<>();
                        //cfg.AddConsumer<>()
                        //cfg.AddRequestClient<AddInitialMovieToThumbnailQueue>();
                    });
                });
    }
}
