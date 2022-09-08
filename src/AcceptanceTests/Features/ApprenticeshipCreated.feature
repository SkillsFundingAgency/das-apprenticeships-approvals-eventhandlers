Feature: Apprenticeship Created

Scenario: Create and publish approval for created apprenticeship
	Given An apprenticeship has been created as part of the commitments journey
	Then an ApprovalCreatedEvent is published
