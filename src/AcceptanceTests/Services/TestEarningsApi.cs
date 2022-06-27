using System;
using WireMock.Server;

namespace SFA.DAS.Apprenticeships.Approvals.EventHandlers.Functions.AcceptanceTests.Services
{
    public class TestEarningsApi : IDisposable
    {
        private bool isDisposed;

        public string BaseAddress { get; private set; }

        public WireMockServer MockServer { get; private set; }

        public TestEarningsApi()
        {
            MockServer = WireMockServer.Start(ssl: false);
            BaseAddress = MockServer.Urls[0];
            BaseAddress += "/api";
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (isDisposed) return;

            if (disposing)
            {
                if (MockServer.IsStarted)
                {
                    MockServer.Stop();
                }
                MockServer.Dispose();
            }

            isDisposed = true;
        }
    }
}
