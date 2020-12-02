using MassTransit;
using GreenPipes;
using System.Threading.Tasks;

public class PublishContextFilter : IFilter<PublishContext>
{
    public void Probe(ProbeContext context)
    {
        context.CreateFilterScope("TRANSACTION");
    }

    public Task Send(PublishContext context, IPipe<PublishContext> next)
    {
        context.Headers.Set
    }
}