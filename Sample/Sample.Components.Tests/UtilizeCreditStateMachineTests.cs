using MassTransit;
using MassTransit.Testing;
using Sample.Contracts.UtilizeCredit;
using Sample.Contracts.UtilizeCredit.CreateCreditEvents;
using Sample.Service.Sagas;
using Sample.TestFramework;
using System;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Sample.Components.Tests
{
    public class UtilizeCreditStateMachineTests : StateMachineTestFixture<UtilizeCreditStateMachine, UtilizeCredit>
    {
        public UtilizeCreditStateMachineTests(ITestOutputHelper testOutputHelper)
            : base(testOutputHelper) 
        {
        }

        [Fact]
        public async Task When_CreditSumIsUnder1000_ShouldReturnValidationError()
        {
            Guid externalId = NewId.NextGuid();

            await StartHarness(harness => harness.OnConfigureInMemoryBus += bus => 
            {

                bus.ReceiveEndpoint(
                    "CreateCredit",
                    cfg => cfg.Handler<CreateCredit>(x => x.Publish<CreateCreditFaulted>(new
                    {
                        ExternalId = externalId.ToString(),
                        ValidationError = "boom"
                    })));   
            });

            var response = await TestHarness.Bus.Request<UtilizeCreditRequested, UtilizeCreditFaulted>(new { CreateCredit = new CreateCreditDTO() { ExternalId = externalId.ToString(), Sum = 999 } });

            Assert.True(await TestHarness.Consumed.Any<UtilizeCreditRequested>(), "Message not consumed");

            //Assert.True(await SagaHarness.Created.Any(x => x.CorrelationId == externalId));

            Assert.True(response.Message.ValidationError == "boom");
        }

        [Fact]
        public async Task When_CreditSumIsAbove000_ShouldStayInOtherState()
        {
            Guid externalId = NewId.NextGuid();

            await StartHarness(harness => harness.OnConfigureInMemoryBus += bus =>
            {

                bus.ReceiveEndpoint(
                    "CreateCredit",
                    cfg => cfg.Handler<CreateCredit>(x => x.Publish<CreateCreditCompleted>(new
                    {
                        ExternalId = externalId.ToString(),
                        TotalDue = 1400
                    })));
            });

             await TestHarness.Bus.Publish<UtilizeCreditRequested>(new
             {
                 CreateCredit = new CreateCreditDTO() { ExternalId = externalId.ToString(), Sum = 1001 },
                 BonusPoints = default(BonusPointDTO)
             });

            Assert.True(await TestHarness.Consumed.Any<UtilizeCreditRequested>(), "Message not consumed");

            var instance = SagaHarness.Created.ContainsInState(externalId, Machine, Machine.CreditCreated);

        }
    }
}
