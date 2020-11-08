﻿using Microsoft.Extensions.Hosting;
using MassTransit;
using Sample.Components;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MassTransit.Definition;
using Sample.Service.Consumers;
using Serilog;
using System.Reflection;

namespace Sample.Service
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            LoggerFactory.SetupMassTransitLogger();

            var builder = new HostBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    services.TryAddSingleton(KebabCaseEndpointNameFormatter.Instance);

                    services.AddMassTransit((cfg) =>
                    {
                        cfg.UsingRabbitMq(BusFactory.CreateBus);
                        cfg.AddConsumers(Assembly.GetExecutingAssembly());
                    });

                    services.AddHostedService<MassTransitConsoleHostedService>();
                })
                .ConfigureLogging((hostingContext, logging) => { logging.AddSerilog(dispose: true); });

            await builder.RunConsoleAsync();
        }
    }
}