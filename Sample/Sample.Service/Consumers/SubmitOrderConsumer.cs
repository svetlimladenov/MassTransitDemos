using MassTransit;
using Microsoft.Extensions.Logging;
using Sample.Contracts;
using System.Threading.Tasks;

namespace Sample.Service.Consumers
{
    public class SubmitOrderConsumer : IConsumer<SubmitOrder>
    {
        private readonly ILogger<SubmitOrderConsumer> logger;

        public SubmitOrderConsumer(ILogger<SubmitOrderConsumer> logger)
        {
            this.logger = logger;
        }

        public async Task Consume(ConsumeContext<SubmitOrder> context)
        {
            this.logger.LogWarning($"SubmitOrderConsumer: {context.Message.CustomerNumber}");

            //await Task.Delay(3000);

            if (!context.Message.CustomerNumber.Contains("TEST"))
            {
                if (context.RequestId != null)
                    await context.RespondAsync<OrderSubmissionAccepted>(new
                    {
                        InVar.Timestamp,
                        context.Message.OrderId,
                        context.Message.CustomerNumber
                    });
            }
            else
            {
                if (context.RequestId != null)
                    await context.RespondAsync<OrderSubmissionRejected>(new
                    {
                        InVar.Timestamp,
                        context.Message.OrderId,
                        context.Message.CustomerNumber,
                        Reason = $"Test Customer cannot submit orders: {context.Message.CustomerNumber}"
                    });

                return;
            }
        }
    }
}
