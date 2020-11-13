using MassTransit.Courier;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Sample.Service.Activities
{
    public class FakeFaultActivity : IExecuteActivity<FakeFaultActivityArgs>
    {
        private readonly ILogger<FakeFaultActivity> logger;

        public FakeFaultActivity(ILogger<FakeFaultActivity> logger)
        {
            this.logger = logger;
        }

        public async Task<ExecutionResult> Execute(ExecuteContext<FakeFaultActivityArgs> context)
        {
            throw new System.Exception("basi");
            this.logger.LogDebug("Fake Activity passing");

            return context.Completed();
        }
    }

    public interface FakeFaultActivityArgs
    {
        int CreditId { get; }
    }
}
