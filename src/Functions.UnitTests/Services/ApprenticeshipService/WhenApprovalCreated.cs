using AutoFixture;
using Moq;
using NServiceBus;
using NUnit.Framework;
using SFA.DAS.Approvals.EventHandlers.Messages;
using SFA.DAS.CommitmentsV2.Messages.Events;
using SFA.DAS.CommitmentsV2.Types;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.Apprenticeships.Approvals.EventHandlers.Functions.UnitTests.Services.ApprenticeshipService
{
    public class WhenApprovalCreated
    {
        private Mock<IMessageSession> _eventPublisher = null!;
        private Functions.Services.ApprenticeshipService _apprenticeshipService = null!;
        private Fixture _fixture = null!;

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture();

            _eventPublisher = new Mock<IMessageSession>();
            _apprenticeshipService = new Functions.Services.ApprenticeshipService(_eventPublisher.Object);
        }

        [Test]
        public async Task ThenApprovalIsCreated()
        {
            var uln = _fixture.Create<string>();
            var firstName = _fixture.Create<string>();
            var lastName = _fixture.Create<string>();
            var apprenticeshipId = _fixture.Create<long>();
            var providerId = _fixture.Create<long>();
            var accountId = _fixture.Create<long>();
            var legalEntityName = _fixture.Create<string>();
            var startDate = _fixture.Create<DateTime?>();
            var endDate = _fixture.Create<DateTime>();
            var transferSenderId = _fixture.Create<long>();
            var employerType = ApprenticeshipEmployerType.Levy;
            var priceEpisodes = new[] { _fixture.Create<PriceEpisode>() };
            var trainingCode = _fixture.Create<string>();
            var dateOfBirth = _fixture.Create<DateTime>();
            var actualStartDate = _fixture.Create<DateTime?>();
            var isOnFlexiPaymentsPilot = _fixture.Create<bool?>();

            await _apprenticeshipService.CreateApproval(uln, firstName, lastName, apprenticeshipId, providerId, accountId, legalEntityName, endDate, transferSenderId, employerType, priceEpisodes, trainingCode, dateOfBirth, startDate, actualStartDate, isOnFlexiPaymentsPilot);

            _eventPublisher.Verify(x => x.Publish(It.Is<ApprovalCreatedEvent>(
                y => y.StartDate == startDate && 
                     y.ApprovalsApprenticeshipId == apprenticeshipId && 
                     y.EmployerAccountId == accountId && 
                     y.FundingEmployerAccountId == transferSenderId &&
                     y.LegalEntityName == legalEntityName &&
                     y.Uln == uln &&
                     y.FirstName == firstName &&
                     y.LastName == lastName &&
                     y.PlannedEndDate == endDate &&
                     y.UKPRN == providerId &&
                     y.AgreedPrice == priceEpisodes.Single().Cost &&
                     y.TrainingCode == trainingCode &&
                     y.DateOfBirth == dateOfBirth &&
                     y.ActualStartDate == actualStartDate &&
                     y.IsOnFlexiPaymentPilot == isOnFlexiPaymentsPilot
                ), It.IsAny<PublishOptions>()));
        }

        [Test]
        public async Task AndTheTransferSenderIsSetThenApprovalIsCreatedWithATransferFundingType()
        {
            var uln = _fixture.Create<string>();
            var firstName = _fixture.Create<string>();
            var lastName = _fixture.Create<string>();
            var apprenticeshipId = _fixture.Create<long>();
            var providerId = _fixture.Create<long>();
            var accountId = _fixture.Create<long>();
            var legalEntityName = _fixture.Create<string>();
            var startDate = _fixture.Create<DateTime?>();
            var endDate = _fixture.Create<DateTime>();
            var transferSenderId = _fixture.Create<long>();
            var employerType = ApprenticeshipEmployerType.Levy;
            var priceEpisodes = new[] { _fixture.Create<PriceEpisode>() };
            var trainingCode = _fixture.Create<string>();
            var dateOfBirth = _fixture.Create<DateTime>();
            var actualStartDate = _fixture.Create<DateTime?>();
            var isOnFlexiPaymentsPilot = _fixture.Create<bool?>();

            await _apprenticeshipService.CreateApproval(uln, firstName, lastName, apprenticeshipId, providerId, accountId, legalEntityName, endDate, transferSenderId, employerType, priceEpisodes, trainingCode, dateOfBirth, startDate, actualStartDate, isOnFlexiPaymentsPilot);

            _eventPublisher.Verify(x => x.Publish(It.Is<ApprovalCreatedEvent>(
                y => y.FundingType == FundingType.Transfer
            ), It.IsAny<PublishOptions>()));
        }

        [Test]
        public async Task AndTheEmployerTypeIsLevyThenApprovalIsCreatedWithALevyFundingType()
        {
            var uln = _fixture.Create<string>();
            var firstName = _fixture.Create<string>();
            var lastName = _fixture.Create<string>();
            var apprenticeshipId = _fixture.Create<long>();
            var providerId = _fixture.Create<long>();
            var accountId = _fixture.Create<long>();
            var legalEntityName = _fixture.Create<string>();
            var startDate = _fixture.Create<DateTime?>();
            var endDate = _fixture.Create<DateTime>();
            var transferSenderId = (long?)null;
            var employerType = ApprenticeshipEmployerType.Levy;
            var priceEpisodes = new[] { _fixture.Create<PriceEpisode>() };
            var trainingCode = _fixture.Create<string>();
            var dateOfBirth = _fixture.Create<DateTime>();
            var actualStartDate = _fixture.Create<DateTime?>();
            var isOnFlexiPaymentsPilot = _fixture.Create<bool?>();

            await _apprenticeshipService.CreateApproval(uln, firstName, lastName, apprenticeshipId, providerId, accountId, legalEntityName,
                endDate, transferSenderId, employerType, priceEpisodes, trainingCode, dateOfBirth, startDate, actualStartDate, isOnFlexiPaymentsPilot);

            _eventPublisher.Verify(x => x.Publish(
                It.Is<ApprovalCreatedEvent>(y => y.FundingType == FundingType.Levy), It.IsAny<PublishOptions>()));
        }

        [Test]
        public async Task AndTheEmployerTypeIsNonLevyThenApprovalIsCreatedWithANonLevyFundingType()
        {
            var uln = _fixture.Create<string>();
            var firstName = _fixture.Create<string>();
            var lastName = _fixture.Create<string>();
            var apprenticeshipId = _fixture.Create<long>();
            var providerId = _fixture.Create<long>();
            var accountId = _fixture.Create<long>();
            var legalEntityName = _fixture.Create<string>();
            var startDate = _fixture.Create<DateTime?>();
            var endDate = _fixture.Create<DateTime>();
            var transferSenderId = (long?)null;
            var employerType = ApprenticeshipEmployerType.NonLevy;
            var priceEpisodes = new[] { _fixture.Create<PriceEpisode>() };
            var trainingCode = _fixture.Create<string>();
            var dateOfBirth = _fixture.Create<DateTime>();
            var actualStartDate = _fixture.Create<DateTime?>();
            var isOnFlexiPaymentsPilot = _fixture.Create<bool?>();

            await _apprenticeshipService.CreateApproval(uln, firstName, lastName, apprenticeshipId, providerId, accountId, legalEntityName,  endDate, transferSenderId, employerType, priceEpisodes, trainingCode, dateOfBirth, startDate, actualStartDate, isOnFlexiPaymentsPilot);

            _eventPublisher.Verify(x => x.Publish(It.Is<ApprovalCreatedEvent>(
                y => y.FundingType == FundingType.NonLevy
            ), It.IsAny<PublishOptions>()));
        }

        [Test]
        public void AndTheEmployerTypeIsNullThenAnExceptionIsThrown()
        {
            var uln = _fixture.Create<string>();
            var firstName = _fixture.Create<string>();
            var lastName = _fixture.Create<string>();
            var apprenticeshipId = _fixture.Create<long>();
            var providerId = _fixture.Create<long>();
            var accountId = _fixture.Create<long>();
            var legalEntityName = _fixture.Create<string>();
            var startDate = _fixture.Create<DateTime?>();
            var endDate = _fixture.Create<DateTime>();
            var transferSenderId = (long?)null;
            var employerType = (ApprenticeshipEmployerType?)null;
            var priceEpisodes = new[] { _fixture.Create<PriceEpisode>() };
            var trainingCode = _fixture.Create<string>();
            var dateOfBirth = _fixture.Create<DateTime>();
            var actualStartDate = _fixture.Create<DateTime?>();
            var isOnFlexiPaymentsPilot = _fixture.Create<bool?>();

            Assert.ThrowsAsync<ArgumentException>(() => _apprenticeshipService.CreateApproval(uln, firstName, lastName, apprenticeshipId, providerId, accountId, legalEntityName, endDate, transferSenderId, employerType, priceEpisodes, trainingCode, dateOfBirth, startDate, actualStartDate, isOnFlexiPaymentsPilot));
        }

        [Test]
        public void AndThereAreMultiplePriceEpisodesThenAnExceptionIsThrown()
        {
            var uln = _fixture.Create<string>();
            var firstName = _fixture.Create<string>();
            var lastName = _fixture.Create<string>();
            var apprenticeshipId = _fixture.Create<long>();
            var providerId = _fixture.Create<long>();
            var accountId = _fixture.Create<long>();
            var legalEntityName = _fixture.Create<string>();
            var startDate = _fixture.Create<DateTime?>();
            var endDate = _fixture.Create<DateTime>();
            var transferSenderId = (long?)null;
            var employerType = ApprenticeshipEmployerType.Levy;
            var priceEpisodes = new[] { _fixture.Create<PriceEpisode>(), _fixture.Create<PriceEpisode>() };
            var trainingCode = _fixture.Create<string>();
            var dateOfBirth = _fixture.Create<DateTime>();
            var actualStartDate = _fixture.Create<DateTime?>();
            var isOnFlexiPaymentsPilot = _fixture.Create<bool?>();

            Assert.ThrowsAsync<ArgumentException>(() => _apprenticeshipService.CreateApproval(uln, firstName, lastName, apprenticeshipId, providerId, accountId, legalEntityName, endDate, transferSenderId, employerType, priceEpisodes, trainingCode, dateOfBirth, startDate, actualStartDate, isOnFlexiPaymentsPilot));
        }

        [Test]
        public void AndThereAreNoPriceEpisodesThenAnExceptionIsThrown()
        {
            var uln = _fixture.Create<string>();
            var firstName = _fixture.Create<string>();
            var lastName = _fixture.Create<string>();
            var apprenticeshipId = _fixture.Create<long>();
            var providerId = _fixture.Create<long>();
            var accountId = _fixture.Create<long>();
            var legalEntityName = _fixture.Create<string>();
            var startDate = _fixture.Create<DateTime?>();
            var endDate = _fixture.Create<DateTime>();
            var transferSenderId = (long?)null;
            var employerType = ApprenticeshipEmployerType.Levy;
            var priceEpisodes = Array.Empty<PriceEpisode>();
            var trainingCode = _fixture.Create<string>();
            var dateOfBirth = _fixture.Create<DateTime>();
            var actualStartDate = _fixture.Create<DateTime?>();
            var isOnFlexiPaymentsPilot = _fixture.Create<bool?>();

            Assert.ThrowsAsync<ArgumentException>(() => _apprenticeshipService.CreateApproval(uln, firstName, lastName, apprenticeshipId, providerId, accountId, legalEntityName, endDate, transferSenderId, employerType, priceEpisodes, trainingCode, dateOfBirth, startDate, actualStartDate, isOnFlexiPaymentsPilot));
        }
    }
}