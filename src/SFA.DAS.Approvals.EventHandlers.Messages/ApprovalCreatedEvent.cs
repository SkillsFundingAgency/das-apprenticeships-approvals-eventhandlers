﻿namespace SFA.DAS.Approvals.EventHandlers.Messages
{
    public class ApprovalCreatedEvent
    {
        public string? Uln { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? TrainingCode { get; set; }
        public long ApprovalsApprenticeshipId { get; set; }
        public long UKPRN { get; set; }
        public long EmployerAccountId { get; set; }
        public string? LegalEntityName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? ActualStartDate { get; set; }
        public DateTime? PlannedEndDate { get; set; }
        public decimal? TrainingPrice { get; set; }
        public decimal? EndPointAssessmentPrice { get; set; }
        public decimal AgreedPrice { get; set; }
        public long? FundingEmployerAccountId { get; set; }
        public FundingType FundingType { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool? IsOnFlexiPaymentPilot { get; set; }
        public string ApprenticeshipHashedId { get; set; }
        public long AccountLegalEntityId { get; set; }
        public string TrainingCourseVersion { get; set; }
    }
}