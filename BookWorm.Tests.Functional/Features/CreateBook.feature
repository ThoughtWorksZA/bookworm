@Specs
Feature: Create Book
	As a Puku admin
	I want to add a book

Scenario: See Create a Book page
	Given I am logged in as an admin
	When I go to Create New Book page 
	Then I see Create New Book page

Scenario: Create a Book
	Given I am on Create New Book page
	When I click create after filling the form
	Then I see the details of the newly created book
	