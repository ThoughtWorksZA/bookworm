@Specs
Feature: Create A Static Page
	As a PUKU administrator
	I want to create a static page
	So that I can publish information relevant to users of the website

Scenario: See Create a Static Page page
	Given I am logged in as an admin
	When I go to Create New Static Page view
	Then I see Create New Static Page view

Scenario: Create a Static Page
	Given I am on Create Static Page view
	When I click create after filling the new page form
	Then I see the details of the newly created page
	