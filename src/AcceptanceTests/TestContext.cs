using System;
using System.Collections.Generic;
using System.IO;
using SFA.DAS.Apprenticeships.Approvals.EventHandlers.Functions.AcceptanceTests.Hooks;
using SFA.DAS.Apprenticeships.Approvals.EventHandlers.Functions.AcceptanceTests.Services;

namespace SFA.DAS.Apprenticeships.Approvals.EventHandlers.Functions.AcceptanceTests
{
    public class TestContext : IDisposable
    {
        public DirectoryInfo TestDirectory { get; set; }
        public TestMessageBus? TestMessageBus { get; set; }
        public TestEarningsApi? EarningsApi { get; set; }
        public TestData TestData { get; set; }
        public List<IHook> Hooks { get; set; }

        public TestContext()
        {
            TestDirectory = new DirectoryInfo(Path.Combine(Directory.GetCurrentDirectory(), Guid.NewGuid().ToString()));
            if (!TestDirectory.Exists)
            {
                Directory.CreateDirectory(TestDirectory.FullName);
            }
            TestData = new TestData();
            Hooks = new List<IHook>();
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


