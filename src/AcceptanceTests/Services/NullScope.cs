using System;

namespace SFA.DAS.Apprenticeships.Approvals.EventHandlers.AcceptanceTests.Services
{
    public class NullScope : IDisposable
    {
        public static NullScope Instance { get; } = new NullScope();

        private NullScope() { }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
