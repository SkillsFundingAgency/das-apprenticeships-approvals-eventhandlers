using System;
using Microsoft.Extensions.Logging;

namespace SFA.DAS.Apprenticeships.Approvals.EventHandlers.Functions.AcceptanceTests.Services
{
    public class TestLogger : ILogger
    {
        public IDisposable BeginScope<TState>(TState state) => NullScope.Instance;

        public bool IsEnabled(LogLevel logLevel) => false;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception, string> formatter)
        {
            
        }
    }
}
