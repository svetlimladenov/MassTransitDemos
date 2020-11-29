using MassTransit;
using Sample.Contracts.UtilizeCredit;
using Sample.Contracts.UtilizeCredit.CreateCreditEvents;
using System.Threading.Tasks;

namespace Sample.Service.Consumers
{
    public class CreateCreditConsumer : IConsumer<CreateCredit>
    {
        public async Task Consume(ConsumeContext<CreateCredit> context)
        {
            await Task.Delay(5000);

            if (context.Message.CreateCredit.Sum >= 1000)
            {
                await context.Publish<CreateCreditCompleted>(new
                {
                    context.Message.CreateCredit.ExternalId,
                    TotalDue = context.Message.CreateCredit.Sum + 125
                });
            }
            else
            {
                await context.Publish<CreateCreditFaulted>(new
                {
                    context.Message.CreateCredit.ExternalId,
                    ValidationError = "Smth went wrong :("
                });
            }
        }
    }
}
