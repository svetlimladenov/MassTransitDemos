using MassTransit;
using MassTransit.RabbitMqTransport;
using System;

namespace Library.Infrastructure
{
    public static class BusFactory
    {
        public static void ConfigureBus(IBusRegistrationContext context, IRabbitMqBusFactoryConfigurator configurator)
        {
            configurator.Host(new Uri("rabbitmq://localhost"), h =>
            {
                h.Username("guest");
                h.Password("guest");
            });
            configurator.ConfigureEndpoints(context);
        }
    }
}
