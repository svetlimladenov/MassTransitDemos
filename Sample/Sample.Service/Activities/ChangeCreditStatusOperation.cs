using MassTransit.Courier;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Sample.Service.Activities
{
    public class ChangeCreditStatusOperationActivity : IActivity<ChangeCreditStatusOperationArgs, ChangeCreditStatusOperationLogs>
    {
        private readonly ILogger<ChangeCreditStatusOperationActivity> logger;

        public ChangeCreditStatusOperationActivity(ILogger<ChangeCreditStatusOperationActivity> logger)
        {
            this.logger = logger;
        }

        public async Task<ExecutionResult> Execute(ExecuteContext<ChangeCreditStatusOperationArgs> context)
        {
            this.logger.LogDebug($"Chaning Credit {context.Arguments.CreditId} Status to {context.Arguments.Status}");
            await Task.Delay(2000);

            return context.Completed(new {
                context.Arguments.CreditId
            });
        }

        public async Task<CompensationResult> Compensate(CompensateContext<ChangeCreditStatusOperationLogs> context)
        {
            this.logger.LogDebug("Reverting Credit Status");
            return context.Compensated();
        }
    }

    public interface ChangeCreditStatusOperationArgs
    {
        int CreditId { get; }

        string Status { get; }
    }

    public interface ChangeCreditStatusOperationLogs
    {
        int CreditId { get; }
    }
}
