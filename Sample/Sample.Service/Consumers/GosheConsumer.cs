using MassTransit;
using Microsoft.Extensions.Logging;
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
        }
    }
}
