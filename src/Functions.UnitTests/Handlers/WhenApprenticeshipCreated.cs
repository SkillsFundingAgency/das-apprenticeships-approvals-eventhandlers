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
        private Mock<IApprenticeshipService> _apprenticeshipService = null!;
        private HandleApprenticeshipCreated _handler = null!;
        private Fixture _fixture = null!;

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
            var @event = _fixture.Create<ApprenticeshipCreatedEvent>();

            await _handler.Handle(@event);

            _apprenticeshipService.Verify(x => x.CreateApproval(@event.Uln, @event.ApprenticeshipId, @event.ProviderId, @event.AccountId, @event.LegalEntityName, @event.StartDate, @event.EndDate, @event.TransferSenderId, @event.ApprenticeshipEmployerTypeOnApproval, @event.PriceEpisodes, @event.TrainingCode));
        }
    }
}