using Sample.Contracts;
using Sample.Service.Consumers;
using Sample.TestFramework;
using System;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Sample.Components.Tests
{
    public class SubmitOrderConsumerTests : ConsumerTestFixture<SubmitOrderConsumer>
    {
        public SubmitOrderConsumerTests(ITestOutputHelper testOutputHelper)
            : base(testOutputHelper)
        {
        }

        [Fact]
        public async Task ShouldRespondAccepted()
        {
            await StartHarness(_ => { });
        }
    }
}
