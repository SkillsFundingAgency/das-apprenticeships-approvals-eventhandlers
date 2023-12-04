using Microsoft.Azure.WebJobs;
using SFA.DAS.Apprenticeships.Approvals.EventHandlers.Functions.Services;
using SFA.DAS.CommitmentsV2.Messages.Events;
using SFA.DAS.NServiceBus.AzureFunction.Attributes;
using System.Threading.Tasks;

namespace SFA.DAS.Apprenticeships.Approvals.EventHandlers.Functions
{
    public class HandleApprenticeshipCreated
    {
        private readonly IApprenticeshipService _apprenticeshipService;

        public HandleApprenticeshipCreated(IApprenticeshipService apprenticeshipService)
        {
            _apprenticeshipService = apprenticeshipService;
        }

        [FunctionName("HandleApprenticeshipCreated")]
        public async Task Handle([NServiceBusTrigger(Endpoint = QueueNames.ApprenticeshipCreated)] ApprenticeshipCreatedEvent @event)
        {
            await _apprenticeshipService.CreateApproval(
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
                @event.ApprenticeshipHashedId);
        }
    }
}