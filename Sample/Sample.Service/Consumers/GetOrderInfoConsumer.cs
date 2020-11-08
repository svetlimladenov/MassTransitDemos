using MassTransit;
using Microsoft.Extensions.Logging;
using Sample.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Service.Consumers
{
    public class GetOrderInfoConsumer : IConsumer<GetOrderInfo>
    {
        private readonly ILogger<GetOrderInfoConsumer> logger;

        public GetOrderInfoConsumer(ILogger<GetOrderInfoConsumer> logger)
        {
            this.logger = logger;
        }

        public async Task Consume(ConsumeContext<GetOrderInfo> context)
        {
            this.logger.LogInformation("Obrabotvam nekvo kuso suobshtenie!");
            await Task.Delay(3000);

            await context.RespondAsync<OrderInfo>(new
            {
                context.Message.OrderId,
                OrderedOn = DateTime.Now,
                Location = "Kyustendil"
            });
        }
    }
}
