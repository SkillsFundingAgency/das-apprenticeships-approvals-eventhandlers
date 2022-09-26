using NServiceBus;
using SFA.DAS.Approvals.EventHandlers.Messages;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace SFA.DAS.Apprenticeships.Approvals.EventHandlers.AcceptanceTests.Handlers;

public class ApprovalCreatedEventHandler : IHandleMessages<ApprovalCreatedEvent>
{
    public static ConcurrentBag<ApprovalCreatedEvent> ReceivedEvents { get; } = new();

    public Task Handle(ApprovalCreatedEvent message, IMessageHandlerContext context)
    {
        ReceivedEvents.Add(message);
        return Task.CompletedTask;
    }
}