Feature: Introductory phone call

In order to provide excellent customer service
As a customer service
I want to arrange a follow up phone call with new customers

Scenario: Follow Up Phonecall on Account Creation

Given I login to CRM
When I navigate to Sales and select Accounts
Then I click on New command
And I set value of name to RandomString of 8
And I set value of telephone1 to '123456789'
And I set value of websiteurl to 'http://dynamics.microsoft.com'
Then I click on Save command
When I go to social pane and select Activities
Then Activity timeline count should be equal to 1