using System.Diagnostics.CodeAnalysis;
using NServiceBus;
using SFA.DAS.Approvals.EventHandlers.Messages;

namespace SFA.DAS.Apprenticeships.Approvals.EventHandlers.Functions
{
    [ExcludeFromCodeCoverage]
    public static class RoutingSettingsExtensions
    {
        public static void AddRouting(this RoutingSettings settings)
        {
            settings.RouteToEndpoint(typeof(ApprovalCreatedEvent), "sfa-das-apprenticeships-approval-created");
        }
    }
}
