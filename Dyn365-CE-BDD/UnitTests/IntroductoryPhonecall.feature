Feature: Introductory phone call

In order to provide excellent customer service
As a customer service
I want to arrange a follow up phone call with new customers

Scenario: Follow Up Phonecall on Account Creation

Given I login to CRM
When I create a new account
Then a phone call record should be created
And due date should be in 5 days