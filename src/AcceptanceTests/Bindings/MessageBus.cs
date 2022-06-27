using System.Threading.Tasks;
using NServiceBus.Transport;
using SFA.DAS.Apprenticeships.Approvals.EventHandlers.Functions.AcceptanceTests.Hooks;
using SFA.DAS.Apprenticeships.Approvals.EventHandlers.Functions.AcceptanceTests.Services;
using TechTalk.SpecFlow;

namespace SFA.DAS.Apprenticeships.Approvals.EventHandlers.Functions.AcceptanceTests.Bindings
{
    [Binding]
    [Scope(Tag = "messageBus")]
    public class MessageBus
    {
        private readonly TestContext _context;

        public MessageBus(TestContext context)
        {
            _context = context;
        }

        [BeforeScenario(Order = 1)]
        public Task InitialiseMessageBus()
        {
            _context.TestMessageBus = new TestMessageBus();
            _context.Hooks.Add(new Hook<MessageContext>());
            return _context.TestMessageBus.Start(_context.TestDirectory);
        }
    }
}
