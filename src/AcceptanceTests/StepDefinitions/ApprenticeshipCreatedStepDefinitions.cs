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
    public class ApprenticeshipCreatedStepDefinitions111
    {
        private static IEndpointInstance? _endpointInstance;
        private readonly ScenarioContext _scenarioContext;
        private readonly TestContext _testContext;
        private readonly Fixture _fixture;

        public ApprenticeshipCreatedStepDefinitions111(ScenarioContext scenarioContext, TestContext testContext)
        {
            _scenarioContext = scenarioContext;
            _testContext = testContext;
            _fixture = new Fixture();
        }

        [BeforeTestRun]
        public static async Task StartEndpoint()
        {
            _endpointInstance = await EndpointHelper
                .StartEndpoint(QueueNames.ApprenticeshipCreated + "test", false, new[] { typeof(ApprenticeshipCreatedEvent), typeof(ApprovalCreatedEvent) });
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

        [Then(@"an ApprovalCreatedEvent is published")]
        public async Task ThenAnApprovalCreatedEventIsPublished()
        {
            await WaitHelper.WaitForIt(() => ApprovalCreatedEventHandler.ReceivedEvents.Any(EventMatchesExpectation), $"Failed to find published {nameof(ApprovalCreatedEvent)}");

            var publishedEvent = ApprovalCreatedEventHandler.ReceivedEvents.Single();

            publishedEvent.ActualStartDate.Should().Be(ApprenticeshipCreatedEvent.StartDate);
            publishedEvent.AgreedPrice.Should().Be(ApprenticeshipCreatedEvent.PriceEpisodes.First().Cost);
            publishedEvent.ApprovalsApprenticeshipId.Should().Be(ApprenticeshipCreatedEvent.ApprenticeshipId);
            publishedEvent.EmployerAccountId.Should().Be(ApprenticeshipCreatedEvent.AccountId);
            publishedEvent.FundingEmployerAccountId.Should().Be(ApprenticeshipCreatedEvent.TransferSenderId);
            publishedEvent.FundingType.Should().Be(FundingType.Transfer);
            publishedEvent.LegalEntityName.Should().Be(ApprenticeshipCreatedEvent.LegalEntityName);
            publishedEvent.PlannedEndDate.Should().Be(ApprenticeshipCreatedEvent.EndDate);
            publishedEvent.TrainingCode.Should().Be(ApprenticeshipCreatedEvent.TrainingCode);
            publishedEvent.UKPRN.Should().Be(ApprenticeshipCreatedEvent.ProviderId);
            publishedEvent.Uln.Should().Be(ApprenticeshipCreatedEvent.Uln);
        }

        private bool EventMatchesExpectation(ApprovalCreatedEvent @event)
        {
            return @event.Uln == ApprenticeshipCreatedEvent.Uln;
        }

        public ApprenticeshipCreatedEvent ApprenticeshipCreatedEvent => (ApprenticeshipCreatedEvent)_scenarioContext["ApprenticeshipCreatedEvent"];

    }
}
