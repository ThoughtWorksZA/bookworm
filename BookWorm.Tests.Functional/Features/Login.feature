@Specs
Feature: Login
	As a Puku user
	I want to login
	So that I can access restricted functionality

Scenario: User is able to log in to the site
	Given I navigate to the login page
	When I enter my credentials
	Then I see a welcome message
	