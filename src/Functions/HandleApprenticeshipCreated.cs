using Microsoft.Azure.WebJobs;
using SFA.DAS.Apprenticeships.Approvals.EventHandlers.Functions.Services;
using SFA.DAS.CommitmentsV2.Messages.Events;
using SFA.DAS.NServiceBus.AzureFunction.Attributes;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace SFA.DAS.Apprenticeships.Approvals.EventHandlers.Functions;

public class HandleApprenticeshipCreated
{
    private readonly IApprenticeshipService _apprenticeshipService;

    public HandleApprenticeshipCreated(IApprenticeshipService apprenticeshipService)
    {
        _apprenticeshipService = apprenticeshipService;
    }

    [FunctionName("HandleApprenticeshipCreated")]
    public async Task Handle([NServiceBusTrigger(Endpoint = QueueNames.ApprenticeshipCreated)] ApprenticeshipCreatedEvent @event,
        ILogger log)
    {
        if (!@event.IsOnFlexiPaymentPilot.GetValueOrDefault())
        {
            log.LogInformation("Apprenticeship {hashedId} is not funded by DAS and therefore, no further processing will occur.", @event.ApprenticeshipHashedId);
            return;
        }

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
            @event.ApprenticeshipHashedId,
            @event.AccountLegalEntityId,
            @event.TrainingCourseVersion);
    }
}