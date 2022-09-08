using SFA.DAS.Apprenticeships.Approvals.EventHandlers.Functions.AcceptanceTests.Services;
using System;
using System.IO;

namespace SFA.DAS.Apprenticeships.Approvals.EventHandlers.Functions.AcceptanceTests
{
    public class TestContext : IDisposable
    {
        public DirectoryInfo TestDirectory { get; set; }
        public TestMessageBus? TestMessageBus { get; set; }
        public TestEarningsApi? EarningsApi { get; set; }
        public TestData TestData { get; set; }
        public TestFunction? TestFunction { get; set; }

        public TestContext()
        {
            TestDirectory = new DirectoryInfo(Path.Combine(Directory.GetCurrentDirectory(), Guid.NewGuid().ToString()));
            if (!TestDirectory.Exists)
            {
                Directory.CreateDirectory(TestDirectory.FullName);
            }
            TestData = new TestData();
        }
        private bool _isDisposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_isDisposed) return;

            if (disposing)
            {
                EarningsApi?.Dispose();
            }

            _isDisposed = true;
        }
    }    
}


