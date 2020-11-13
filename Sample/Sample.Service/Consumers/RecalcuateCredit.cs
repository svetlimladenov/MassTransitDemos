using MassTransit;
using MassTransit.Courier;
using Microsoft.Extensions.Logging;
using Sample.Contracts;
using System;
using System.Threading.Tasks;

namespace Sample.Service.Consumers
{
    public class RecalcuateCredit : IConsumer<RecalculateCreditMessage>
    {
        private readonly ILogger<RecalcuateCredit> logger;

        public RecalcuateCredit(ILogger<RecalcuateCredit> logger)
        {
            this.logger = logger;
        }

        public async Task Consume(ConsumeContext<RecalculateCreditMessage> context)
        {
            var builder = new RoutingSlipBuilder(NewId.NextGuid());

            builder.AddActivity("ChangeCreditStatusOperation", GetExecuteAddress("ChangeCreditStatusOperation"), new { context.Message.CreditId, Status = "Recaulc"});
            builder.AddActivity("FakeFault", GetExecuteAddress("FakeFault"), new { CreditId = context.Message.CreditId });

            var routingSlip = builder.Build();

            await context.Execute(routingSlip);
        }

        private Uri GetExecuteAddress(string queueName)
        {
            return new Uri($"queue:{queueName}_execute");
        }
    }
}
