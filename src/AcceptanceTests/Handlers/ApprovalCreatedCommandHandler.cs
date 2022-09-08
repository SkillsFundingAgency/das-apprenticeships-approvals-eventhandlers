using NServiceBus;
using SFA.DAS.Approvals.EventHandlers.Messages;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace SFA.DAS.Apprenticeships.Approvals.EventHandlers.Functions.AcceptanceTests.Handlers;

public class ApprovalCreatedCommandHandler : IHandleMessages<ApprovalCreatedCommand>
{
    public static ConcurrentBag<ApprovalCreatedCommand> ReceivedCommands { get; } = new();

    public Task Handle(ApprovalCreatedCommand message, IMessageHandlerContext context)
    {
        ReceivedCommands.Add(message);
        return Task.CompletedTask;
    }
}