using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MoveIndexers.LocalFileProcessor
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
                    services.AddSingleton<IInitialLoadSearcher, InitialLoadSearcher>();
                    services.AddMassTransit(_ =>
                    {
                        _.AddBus(factory =>
                        {
                            return Bus.Factory.CreateUsingRabbitMq(rmq =>
                            {
                                rmq.Host("rabbitmq://10.0.0.231/dev");
                            });
                        });
                    });
                });
    }
}
