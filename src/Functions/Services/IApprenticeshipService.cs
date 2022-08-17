using System;
using System.Threading.Tasks;
using SFA.DAS.CommitmentsV2.Messages.Events;
using SFA.DAS.CommitmentsV2.Types;

namespace SFA.DAS.Apprenticeships.Approvals.EventHandlers.Functions.Services
{
    public interface IApprenticeshipService
    {
        Task CreateApproval(string uln, long apprenticeshipId, long ukprn, long employerAccountId, string legalEntityName, DateTime actualStartDate, DateTime plannedEndDate, long? transferSenderId, ApprenticeshipEmployerType? apprenticeshipEmployerType, PriceEpisode[] priceEpisodes, decimal fundingBandMaximum);
    }
}
