Feature: A logged in user can regenerate a single meal
	The logged in user can regenerate a single meal from the meal planner
	Link here: https://www.pivotaltracker.com/story/show/177689443
	Testing/Implementation done by: Trent Farley

Scenario: A logged in user wants to regenerate a meal
	Given a user is on the meal planner page
	And a meal plan is generated
	When a user clicks the regenerate button
	Then a new meal should appear

Scenario: A logged in users does not want to regenerate a meal
	Given a user is logged in and on the meal planning page
	And a meal plan is generated
	When a user does not click the regenerate button
	Then a new meal should not appear