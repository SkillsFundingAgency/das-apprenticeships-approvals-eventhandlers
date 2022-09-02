using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using SFA.DAS.Apprenticeships.Approvals.EventHandlers.Functions.Services;
using SFA.DAS.CommitmentsV2.Messages.Events;
using SFA.DAS.NServiceBus.AzureFunction.Attributes;

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
        public async Task HandleCommand([NServiceBusTrigger(Endpoint = QueueNames.ApprenticeshipCreated)] ApprenticeshipCreatedEvent command)
        {
            await _apprenticeshipService.CreateApproval(
                command.Uln,
                command.ApprenticeshipId,
                command.ProviderId,
                command.AccountId,
                command.LegalEntityName,
                command.StartDate,
                command.EndDate,
                command.TransferSenderId,
                command.ApprenticeshipEmployerTypeOnApproval,
                command.PriceEpisodes,
                command.TrainingCode);
        }
    }
}