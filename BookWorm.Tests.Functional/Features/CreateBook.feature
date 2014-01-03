@Specs
Feature: Create Book
	As a Puku admin
	I want to add a book

Scenario: Create a Book
	Given I am logged in as an admin
	When I go to Create New Book page 
	And I click create after filling the form
	Then I see the details of the newly created book
	