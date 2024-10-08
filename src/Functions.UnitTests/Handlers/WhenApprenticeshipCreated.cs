using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using Microsoft.Extensions.Logging;
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
        public async Task ForDASFundedApprenticeshipThenApprovalIsCreated()
        {
            var @event = _fixture.Build<ApprenticeshipCreatedEvent>().With(x => x.IsOnFlexiPaymentPilot, true).Create();

            await _handler.Handle(@event, new Mock<ILogger>().Object);

            _apprenticeshipService.Verify(x => x.CreateApproval(
                @event.Uln, 
                @event.FirstName, 
                @event.LastName, 
                @event.ApprenticeshipId, 
                @event.ProviderId, 
                @event.AccountId, 
                @event.LegalEntityName, 
                @event.EndDate, 
                @event.TransferSenderId, 
                @event.ApprenticeshipEmployerTypeOnApproval, 
                @event.PriceEpisodes, 
                @event.TrainingCode, 
                @event.DateOfBirth, 
                @event.StartDate, 
                @event.ActualStartDate, 
                @event.IsOnFlexiPaymentPilot, 
                @event.ApprenticeshipHashedId, 
                @event.AccountLegalEntityId, 
                @event.TrainingCourseVersion));
        }

        [TestCase(null)]
        [TestCase(false)]
        public async Task ForSLDFundedApprenticeshipThenApprovalNotCreated(bool? isOnFlexiPaymentPilot)
        {
            var @event = _fixture.Build<ApprenticeshipCreatedEvent>().With(x => x.IsOnFlexiPaymentPilot, isOnFlexiPaymentPilot)
                .Create();

            await _handler.Handle(@event, new Mock<ILogger>().Object);

            _apprenticeshipService.Verify(x => x.CreateApproval(
                @event.Uln,
                @event.FirstName,
                @event.LastName,
                @event.ApprenticeshipId,
                @event.ProviderId,
                @event.AccountId,
                @event.LegalEntityName,
                @event.EndDate,
                @event.TransferSenderId,
                @event.ApprenticeshipEmployerTypeOnApproval,
                @event.PriceEpisodes,
                @event.TrainingCode,
                @event.DateOfBirth,
                @event.StartDate,
                @event.ActualStartDate,
                @event.IsOnFlexiPaymentPilot,
                @event.ApprenticeshipHashedId,
                @event.AccountLegalEntityId,
                @event.TrainingCourseVersion), Times.Never);
        }
    }
}