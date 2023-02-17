using System;
using System.Threading.Tasks;
using SFA.DAS.CommitmentsV2.Messages.Events;
using SFA.DAS.CommitmentsV2.Types;

namespace SFA.DAS.Apprenticeships.Approvals.EventHandlers.Functions.Services
{
    public interface IApprenticeshipService
    {
        Task CreateApproval(string uln, string firstName, string lastName, long apprenticeshipId, long ukprn, long employerAccountId,
            string legalEntityName, DateTime plannedEndDate, long? transferSenderId,
            ApprenticeshipEmployerType? apprenticeshipEmployerType, PriceEpisode[] priceEpisodes, string trainingCode,
            DateTime dateOfBirth, DateTime? startDate, DateTime? actualStartDate, bool? isOnFlexiPaymentPilot);
    }
}
