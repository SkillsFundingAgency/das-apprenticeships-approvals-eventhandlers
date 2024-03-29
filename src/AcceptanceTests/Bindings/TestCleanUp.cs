﻿using System.IO;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace SFA.DAS.Apprenticeships.Approvals.EventHandlers.AcceptanceTests.Bindings
{
    [Binding]
    public class TestCleanUp
    {
        private readonly TestContext _context;
        public TestCleanUp(TestContext context)
        {
            _context = context;
        }

        [AfterScenario()]
        public async Task CleanUp()
        {
            if (_context.TestMessageBus != null &&  _context.TestMessageBus.IsRunning)
            {
                await _context.TestMessageBus.Stop();
            }
            Directory.Delete(_context.TestDirectory.FullName, true);
            _context.EarningsApi?.Dispose();
        }
    }
}
