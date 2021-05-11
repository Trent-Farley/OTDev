Feature: Details of a meal in a modal
	The user can see the details of a meal from the meal planning view
	Link here: https://www.pivotaltracker.com/story/show/177916812
	Testing/Implementation done by: Trent Farley

Scenario: A logged in user wants to see the details of a meal
	Given a user is logged in and on the meal planning page
	And a meal plan is generated
	When a user clicks the info button
	Then a modal should appear with the meal's details

Scenario: A logged in user sees the details of a meal
	Given the details of a meal has already appeared in a modal
	When a user clicks the close button
	Then the modal should close