using Library.Components.StateMachines;
using Library.Infrastructure;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System.Threading.Tasks;

namespace Library.Reservation.Service
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            LoggerFactory.SetupMassTransitLogger();

            var hostBulder = new HostBuilder()
                .ConfigureServices((hostBulder, serviceCollection) =>
                {
                    serviceCollection.AddMassTransit(cfg =>
                    {
                        cfg.AddSagaStateMachine<ReservationStateMachine, Components.StateMachines.Reservation>().InMemoryRepository();

                        cfg.UsingRabbitMq(BusFactory.ConfigureBus);
                    });

                    serviceCollection.AddHostedService<ReservationService>();
                })
                .ConfigureLogging((hostBulder, logging) => logging.AddSerilog());

            await hostBulder.RunConsoleAsync();
        }
    }
}
