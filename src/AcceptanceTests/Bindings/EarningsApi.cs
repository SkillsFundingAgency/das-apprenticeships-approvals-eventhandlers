using SFA.DAS.Apprenticeships.Approvals.EventHandlers.AcceptanceTests.Services;
using TechTalk.SpecFlow;

namespace SFA.DAS.Apprenticeships.Approvals.EventHandlers.AcceptanceTests.Bindings
{
    [Binding]
    [Scope(Tag = "earningsApi")]
    public class EarningsApi
    {
        private readonly TestContext _context;

        public EarningsApi(TestContext context)
        {
            _context = context;
        }

        [BeforeScenario()]
        public void InitialiseApi()
        {
            _context.EarningsApi = new TestEarningsApi();
        }
    }
}
