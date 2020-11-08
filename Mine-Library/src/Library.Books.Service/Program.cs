using Microsoft.Extensions.Hosting;
using MassTransit;
using Library.Infrastructure;
using Serilog;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Library.Components.StateMachines;
using Library.Books.Service.Consumers;
using System.Reflection;

namespace Library.Books.Service
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            LoggerFactory.SetupMassTransitLogger();

            var hostBuilder = new HostBuilder()
                .ConfigureServices((hostBulder, serviceCollection) =>
                {
                    serviceCollection.AddMassTransit(cfg =>
                    {
                        //cfg.AddConsumersFromNamespaceContaining<BookReservedConsumer>();

                        cfg.AddConsumers(Assembly.GetExecutingAssembly());

                        cfg.AddSagaStateMachine<BookStateMachine, Book>().InMemoryRepository();

                        cfg.UsingRabbitMq(BusFactory.ConfigureBus);
                    });

                    serviceCollection.AddHostedService<BooksService>();
                })
                .ConfigureLogging((hostingContext, logging) => 
                {
                    logging.AddSerilog(dispose: true);
                });

            await hostBuilder.RunConsoleAsync();
        }
    }
}
