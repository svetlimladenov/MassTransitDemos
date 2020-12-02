using GreenPipes;
using MassTransit;
using MassTransit.Metadata;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Sample.Components
{
    public class ConsoleConsumerFilter : IFilter<ConsumeContext>
    {
        public void Probe(ProbeContext context)
        {
            context.CreateFilterScope("consoleConsumerFilter");
            context.Add("output", "console");
        }

        public Task Send(ConsumeContext context, IPipe<ConsumeContext> next)  // or we make it async and dont return
        {
            Console.WriteLine($"Consumer: {context.MessageId}");
            return next.Send(context);
        }
    }
    
    public class ConsoleConsumeWithConsumerFilter<TConsumer> : 
        IFilter<ConsumerConsumeContext<TConsumer>>
        where TConsumer : class
    {
        public void Probe(ProbeContext context)
        {
            context.CreateFilterScope("consoleWithConsumerConsumerFilter");
            context.Add("output", "console");
        }

        public Task Send(ConsumerConsumeContext<TConsumer> context, IPipe<ConsumerConsumeContext<TConsumer>> next)
        {
            Console.WriteLine($"Consume with consumer: {context.MessageId}");
            return next.Send(context);
        }
    }

    public class ConsoleConsumeWithConsumerFilter<TConsumer, TMessage> :
        IFilter<ConsumerConsumeContext<TConsumer, TMessage>>
        where TConsumer : class
        where TMessage : class
    {
        public void Probe(ProbeContext context)
        {
            context.CreateFilterScope("consoleWithConsumerConsumerFilter");
            context.Add("output", "console");
        }

        public async Task Send(ConsumerConsumeContext<TConsumer, TMessage> context, IPipe<ConsumerConsumeContext<TConsumer, TMessage>> next)
        {
            Console.WriteLine($"Consume with consumer/message: {context.MessageId} - {TypeMetadataCache<TMessage>.ShortName}");

            var serviceProvider = context.GetPayload<IServiceProvider>();
            var validator = serviceProvider.GetService<IMessageValidator<TMessage>>();
            if(validator != null)
            {
                await validator.Validate(context);
            }
            
            await next.Send(context);
        }
    }

    public interface IMessageValidator<in TMessage>
        where TMessage : class 
    {
        Task Validate(ConsumeContext<TMessage> context);
    }

    public class MessageValidator<TMessage> : IMessageValidator<TMessage>
        where TMessage : class
    {
        public Task Validate(ConsumeContext<TMessage> context)
        {
            Console.WriteLine($"Validated {context.MessageId}");
            return Task.CompletedTask;
        }
    }
}
