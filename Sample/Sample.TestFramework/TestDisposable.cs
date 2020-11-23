using System;

namespace Sample.TestFramework
{
    public partial class TestOutputLogger
    {
        public class TestDisposable : IDisposable
        {
            public static readonly TestDisposable Instance = new TestDisposable();

            public void Dispose()
            {
                // intentionally does nothing
            }
        }
    }
}