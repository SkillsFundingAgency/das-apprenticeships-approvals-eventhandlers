using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.Apprenticeships.Approvals.EventHandlers.Functions.Configuration
{
    [ExcludeFromCodeCoverage]
    public class ApplicationSettings
    {
        public string AzureWebJobsStorage { get; set; }
        public string NServiceBusConnectionString { get; set; }
        public string NServiceBusLicense { get; set; }
        public string LearningTransportStorageDirectory { get; set; }
    }
}
