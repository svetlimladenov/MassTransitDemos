using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace Sample.TestFramework
{
    public class TestOutputLoggerFactory : ILoggerFactory
    {
        private readonly bool enabled;

        public TestOutputLoggerFactory(bool enabled, ITestOutputHelper outputHelper = null)
        {
            this.enabled = enabled;
            Current = outputHelper;
        }

        public ITestOutputHelper Current { get; set; }

        public ILogger CreateLogger(string name)
        {
            return new TestOutputLogger(this, enabled);
        }

        public void AddProvider(ILoggerProvider provider)
        {
        }

        public void Dispose()
        {
        }
    }
}
