using NServiceBus;
using SFA.DAS.Approvals.EventHandlers.Messages;
using SFA.DAS.CommitmentsV2.Messages.Events;
using SFA.DAS.CommitmentsV2.Types;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.Apprenticeships.Approvals.EventHandlers.Functions.Services
{
    public class ApprenticeshipService : IApprenticeshipService
    {
        private readonly IMessageSession _eventPublisher;

        public ApprenticeshipService(IMessageSession eventPublisher)
        {
            _eventPublisher = eventPublisher;
        }

        public async Task CreateApproval(string uln, string firstName, string lastName, long apprenticeshipId, long ukprn, long employerAccountId, string legalEntityName, DateTime plannedEndDate, long? transferSenderId, ApprenticeshipEmployerType? apprenticeshipEmployerType, PriceEpisode[] priceEpisodes, string trainingCode, DateTime dateOfBirth, DateTime? startDate, DateTime? actualStartDate, bool? isOnFlexiPaymentPilot)
        {
            if (apprenticeshipEmployerType == null)
            {
                throw new ArgumentException("ApprenticeshipEmployerType cannot be null");
            }

            if (priceEpisodes.Length != 1)
            {
                throw new ArgumentException("There must be exactly one price episode");
            }

            var fundingType = FundingType.Levy;
            if (transferSenderId.HasValue)
            {
                fundingType = FundingType.Transfer;
            } 
            else if (apprenticeshipEmployerType == ApprenticeshipEmployerType.NonLevy)
            {
                fundingType = FundingType.NonLevy;
            }

            var approvalCreatedEvent = new ApprovalCreatedEvent
            {
                LegalEntityName = legalEntityName,
                AgreedPrice = priceEpisodes[0].Cost,
                ApprovalsApprenticeshipId = apprenticeshipId,
                EmployerAccountId = employerAccountId,
                FundingEmployerAccountId = transferSenderId,
                FundingType = fundingType,
                PlannedEndDate = plannedEndDate,
                TrainingCode = trainingCode,
                UKPRN = ukprn,
                Uln = uln,
                FirstName = firstName,
                LastName = lastName,
                DateOfBirth = dateOfBirth,
                StartDate = startDate,
                ActualStartDate = actualStartDate,
                IsOnFlexiPaymentPilot = isOnFlexiPaymentPilot

            };

            await _eventPublisher.Publish(approvalCreatedEvent);
        }
    }
}
