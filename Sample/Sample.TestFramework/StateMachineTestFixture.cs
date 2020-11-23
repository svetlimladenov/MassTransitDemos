using Automatonymous;
using MassTransit;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using MassTransit.Testing;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Sample.TestFramework
{
    public class StateMachineTestFixture<TStateMachine, TInstance> : BasicHarnessFixture
       where TStateMachine : class, SagaStateMachine<TInstance>
       where TInstance : class, SagaStateMachineInstance
    {
        protected IStateMachineSagaTestHarness<TInstance, TStateMachine> SagaHarness;
        protected TStateMachine Machine;

        public StateMachineTestFixture(ITestOutputHelper testOutputHelper)
            : base(testOutputHelper)
        {
        }

        protected override void Setup()
        {
            base.Setup();
            SagaHarness = Provider.GetRequiredService<IStateMachineSagaTestHarness<TInstance, TStateMachine>>();
            Machine = Provider.GetRequiredService<TStateMachine>();
        }

        protected override void ConfigureMassTransit(IServiceCollectionBusConfigurator configurator)
        {
            configurator.AddSagaStateMachine<TStateMachine, TInstance>()
                      .InMemoryRepository();
            configurator.AddSagaStateMachineTestHarness<TStateMachine, TInstance>();
        }
    }
}
