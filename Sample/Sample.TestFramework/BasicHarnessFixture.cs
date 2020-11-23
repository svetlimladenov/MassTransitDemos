using System;
using System.Threading.Tasks;
using MassTransit.Context;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using MassTransit.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit;
using Xunit.Abstractions;

namespace Sample.TestFramework
{
    public class BasicHarnessFixture : IAsyncLifetime
    {
        public ServiceProvider Provider;
        public InMemoryTestHarness TestHarness;
        public ITestOutputHelper TestOutputHelper;

        public BasicHarnessFixture(ITestOutputHelper TestOutputHelper)
        {
            this.TestOutputHelper = TestOutputHelper;
        }

        protected virtual void Setup()
        {
            var collection = new ServiceCollection()
                .AddSingleton<ILoggerFactory>(provider => new TestOutputLoggerFactory(true, this.TestOutputHelper))
                .AddMassTransitInMemoryTestHarness(cfg =>
                {
                    ConfigureMassTransit(cfg);
                });

            ConfigureServices(collection);
            Provider = collection.BuildServiceProvider(true);
            TestHarness = Provider.GetRequiredService<InMemoryTestHarness>();
        }

        public async Task StartHarness(Action<InMemoryTestHarness> configureHarness)
        {
            ConfigureLogging();
            configureHarness(TestHarness);
            await TestHarness.Start();
        }

        protected virtual void ConfigureMassTransit(IServiceCollectionBusConfigurator configurator)
        {
        }

        protected virtual void ConfigureServices(IServiceCollection collection)
        {
        }

        private void ConfigureLogging()
        {
            var loggerFactory = Provider.GetRequiredService<ILoggerFactory>();

            LogContext.ConfigureCurrentLogContext(loggerFactory);
        }

        public async Task InitializeAsync()
        {
            Setup();
        }

        async Task IAsyncLifetime.DisposeAsync()
        {
            try
            {
                await TestHarness.Stop();
            }
            finally
            {
                await Provider.DisposeAsync();
            }
        }
    }
}
