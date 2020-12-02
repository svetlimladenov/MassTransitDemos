using GreenPipes;
using MassTransit;
using MassTransit.ConsumeConfigurators;
using MassTransit.Definition;
using Microsoft.Extensions.Logging;
using Sample.Components;
using Sample.Contracts;
using System;
using System.Threading.Tasks;

namespace Sample.Service.Consumers
{
    public class GosheConsumer : IConsumer<MessageToGoshe>
    {
        private readonly ILogger<GosheConsumer> logger;

        public GosheConsumer(ILogger<GosheConsumer> logger)
        {
            this.logger = logger;
        }

        public async Task Consume(ConsumeContext<MessageToGoshe> context)
        {
            this.logger.LogError(context.Message.Message);

            await Task.Delay(1000);
        }
    }

    public class GosheConsumerDefinition : ConsumerDefinition<GosheConsumer>
    {
        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<GosheConsumer> consumerConfigurator)
        {
            // These doesnt need a consumer
           // endpointConfigurator.ConnectConsumerConfigurationObserver(new ConsoleConsumerMessageFilterConfigurationObserver(endpointConfigurator));

            endpointConfigurator.UseFilter(new ConsoleConsumerFilter());

            // these needs a consumer
            consumerConfigurator.UseFilter(new ConsoleConsumeWithConsumerFilter<GosheConsumer>());

            consumerConfigurator.ConsumerMessage<MessageToGoshe>(m => m.UseFilter(new ConsoleConsumeWithConsumerFilter<GosheConsumer, MessageToGoshe>()));
        }
    }
}
