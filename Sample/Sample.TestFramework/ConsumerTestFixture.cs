using MassTransit;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using MassTransit.Testing;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace Sample.TestFramework
{
    public class ConsumerTestFixture<TConsumer> : BasicHarnessFixture, IAsyncLifetime
        where TConsumer : class, IConsumer
    {
        public TConsumer Consumer;
        public IConsumerTestHarness<TConsumer> ConsumerHarness;

        public ConsumerTestFixture(ITestOutputHelper TestOutputHelper)
            : base(TestOutputHelper)
        {
        }

        protected override void Setup()
        {
            base.Setup();
            ConsumerHarness = Provider.GetRequiredService<IConsumerTestHarness<TConsumer>>();
        }

        protected override void ConfigureMassTransit(IServiceCollectionBusConfigurator configurator)
        {
            configurator.AddConsumer<TConsumer>();
            configurator.AddConsumerTestHarness<TConsumer>();
        }
    }
}
