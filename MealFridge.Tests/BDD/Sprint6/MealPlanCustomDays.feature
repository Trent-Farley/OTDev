Feature: A logged in user wants to generate a meal with up to 14 days of meals
	The logged in user can generate meals up to two weeks
	Link here: https://www.pivotaltracker.com/story/show/177689443
	Testing/Implementation done by: Trent Farley

Scenario Outline: A user requests for any amount of days' worth of meals, up to 14 days
	Given a user is logged in
	And on the meal planning page
	When a user specifies <days> worth of days to generate
	Then a meal plan should be generated with <days> worth of meals

	Examples:
		| days |
		| 5    |
		| 11   |
		| 2    |
		| 14   |