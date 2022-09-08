using System.IO;
using System.Threading.Tasks;
using NServiceBus;
using SFA.DAS.NServiceBus.Configuration;
using SFA.DAS.NServiceBus.Configuration.NewtonsoftJsonSerializer;

namespace SFA.DAS.Apprenticeships.Approvals.EventHandlers.AcceptanceTests.Services
{
    public class TestMessageBus
    {
        private IEndpointInstance? _endpointInstance;
        public bool IsRunning { get; private set; }
        public DirectoryInfo? StorageDirectory { get; private set; }
        public async Task Start(DirectoryInfo testDirectory)
        {
            StorageDirectory = new DirectoryInfo(Path.Combine(testDirectory.FullName, ".learningtransport"));
            if (!StorageDirectory.Exists)
            {
                Directory.CreateDirectory(StorageDirectory.FullName);
            }

            var endpointConfiguration = new EndpointConfiguration("SFA.DAS.Funding.ApprenticeshipEarnings.Approvals.EventHandlers.Functions.TestMessageBus");
            endpointConfiguration
                .UseNewtonsoftJsonSerializer()
                .UseMessageConventions()
                .UseTransport<LearningTransport>()
                .StorageDirectory(StorageDirectory.FullName);
            endpointConfiguration.UseLearningTransport();

            _endpointInstance = await Endpoint.Start(endpointConfiguration).ConfigureAwait(false);
            IsRunning = true;
        }

        public async Task Stop()
        {
            await _endpointInstance?.Stop()!;
            IsRunning = false;
        }

        public Task Publish(object message)
        {
            return _endpointInstance.Publish(message);
        }

        public Task Send(object message)
        {
            return _endpointInstance.Send(message);
        }

    }
}
