using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.Apprenticeships.Approvals.EventHandlers.Functions.Services;
using SFA.DAS.CommitmentsV2.Messages.Events;

namespace SFA.DAS.Apprenticeships.Approvals.EventHandlers.Functions.UnitTests.Handlers
{
    public class WhenApprenticeshipCreated
    {
        private Mock<IApprenticeshipService> _apprenticeshipService;
        private HandleApprenticeshipCreated _handler;
        private Fixture _fixture;

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture();

            _apprenticeshipService = new Mock<IApprenticeshipService>();
            _handler = new HandleApprenticeshipCreated(_apprenticeshipService.Object);
        }

        [Test]
        public async Task ThenApprovalIsCreated()
        {
            var command = _fixture.Create<ApprenticeshipCreatedEvent>();

            await _handler.HandleCommand(command);

            _apprenticeshipService.Verify(x => x.CreateApproval(command.Uln, command.ApprenticeshipId, command.ProviderId, command.AccountId, command.LegalEntityName, command.StartDate, command.EndDate, command.TransferSenderId, command.ApprenticeshipEmployerTypeOnApproval, command.PriceEpisodes));
        }
    }
}