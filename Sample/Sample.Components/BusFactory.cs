using MassTransit;
using MassTransit.RabbitMqTransport;
using System;

namespace Sample.Components
{
    public class BusFactory
    {
        public static void CreateBus(IBusRegistrationContext context, IRabbitMqBusFactoryConfigurator configurator)
        {
            configurator.Host(new Uri("rabbitmq://localhost/test"), h =>
            {
                h.Username("guest");
                h.Password("guest");
            });

            // When you add Consumers, sagas etc to to the container, MassTransit keeps track of all this registrations
            // So when you call ConfigureEndpoints by convention it will configure all the names of the queues, 
            // which consumers to put on the queues
            configurator.ConfigureEndpoints(context);
        }
    }
}
