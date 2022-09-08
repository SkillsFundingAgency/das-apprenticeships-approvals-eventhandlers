using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using NServiceBus;
using SFA.DAS.Apprenticeships.Approvals.EventHandlers.Functions.AcceptanceTests.Handlers;
using SFA.DAS.Apprenticeships.Approvals.EventHandlers.Functions.AcceptanceTests.Helpers;
using SFA.DAS.Approvals.EventHandlers.Messages;
using SFA.DAS.CommitmentsV2.Messages.Events;
using TechTalk.SpecFlow;

namespace SFA.DAS.Apprenticeships.Approvals.EventHandlers.Functions.AcceptanceTests.StepDefinitions
{
    [Binding]
    public class ApprenticeshipCreatedStepDefinitions
    {
        private static IEndpointInstance? _endpointInstance;
        private readonly ScenarioContext _scenarioContext;
        private readonly TestContext _testContext;
        private readonly Fixture _fixture;

        public ApprenticeshipCreatedStepDefinitions(ScenarioContext scenarioContext, TestContext testContext)
        {
            _scenarioContext = scenarioContext;
            _testContext = testContext;
            _fixture = new Fixture();
        }

        [BeforeTestRun]
        public static async Task StartEndpoint()
        {
            _endpointInstance = await EndpointHelper
                .StartEndpoint(QueueNames.ApprenticeshipCreated + "TEST", false, new[] { typeof(ApprenticeshipCreatedEvent), typeof(ApprovalCreatedCommand) });
        }

        [AfterTestRun]
        public static async Task StopEndpoint()
        {
            await _endpointInstance!.Stop()
                .ConfigureAwait(false);
        }

        [Given(@"An apprenticeship has been created as part of the commitments journey")]
        public async Task GivenAnApprenticeshipHasBeenCreatedAsPartOfTheCommitmentsJourney()
        {
            var apprenticeshipCreatedEvent = _fixture.Build<ApprenticeshipCreatedEvent>()
                .With(_ => _.Uln, _fixture.Create<long>().ToString)
                .With(_ => _.TrainingCode, _fixture.Create<string>()[..10])
                .With(_ => _.PriceEpisodes, _fixture.CreateMany<PriceEpisode>(1).ToArray)
                .Create();

            await _endpointInstance.Publish(apprenticeshipCreatedEvent);

            _scenarioContext["ApprenticeshipCreatedEvent"] = apprenticeshipCreatedEvent;
        }

        [Then(@"an ApprovalCreatedCommand is published")]
        public async Task ThenAnApprovalCreatedCommandIsPublished()
        {
            await WaitHelper.WaitForIt(() => ApprovalCreatedCommandHandler.ReceivedCommands.Any(EventMatchesExpectation), $"Failed to find published {nameof(ApprovalCreatedCommand)}");

            var publishedEvent = ApprovalCreatedCommandHandler.ReceivedCommands.Single();

            publishedEvent.Should().BeEquivalentTo(ApprenticeshipCreatedEvent);
        }

        private bool EventMatchesExpectation(ApprovalCreatedCommand command)
        {
            return command.Uln == ApprenticeshipCreatedEvent.Uln;
        }

        public ApprenticeshipCreatedEvent ApprenticeshipCreatedEvent => (ApprenticeshipCreatedEvent)_scenarioContext["ApprenticeshipCreatedEvent"];

    }
}
