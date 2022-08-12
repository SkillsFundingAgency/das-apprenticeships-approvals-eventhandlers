using System.Threading.Tasks;
using SFA.DAS.Apprenticeships.Approvals.EventHandlers.Functions.AcceptanceTests.Services;
using TechTalk.SpecFlow;

namespace SFA.DAS.Apprenticeships.Approvals.EventHandlers.Functions.AcceptanceTests.Bindings
{
    [Binding]
    public class Functions
    {
        private readonly TestContext _context;

        public Functions(TestContext context)
        {
            _context = context;
        }

        [BeforeScenario()]
        public async Task InitialiseFunctions()
        {
        }
    }
}