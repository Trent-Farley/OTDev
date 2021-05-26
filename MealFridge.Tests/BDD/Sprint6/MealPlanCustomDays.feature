Feature: A logged in user wants to generate a meal with up to 14 days of meals
	The logged in user can generate meals up to two weeks
	Link here: https://www.pivotaltracker.com/story/show/177689443
	Testing/Implementation done by: Trent Farley

Scenario: A user requests a meal plan for 5 days worth of meals
	Given the meal planner page is active
	When the add filters button is clicked
	And a user specifies 5 worth of days to generate
	Then a meal plan should be generated with 5 worth of meals

Scenario: A user requests a meal plan for 14 days worth of meals
	Given the meal planner page is active
	When the add filters button is clicked
	When a user specifies 14 worth of days to generate
	Then a meal plan should be generated with 14 worth of meals