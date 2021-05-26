Feature: A logged in user would like to be able to filter a meal plan by characteristics of a recipe
	Link here: https://www.pivotaltracker.com/story/show/178102723
	Testing/Implementation done by: Trent Farley

Scenario Outline: A user generates a meal plan with custom filters
	Given the meal planner page is active
	When the add filters button is clicked
	And fat is set to <Fat>
	And the generate meal plan button is clicked
	Then a customized meal plan should be created with 7 days worth of meals

	# Second scenario outline with assertion for total to be less then provided filters
	Examples:
		| Fat |
		| 20  |
		| 60  |

Scenario: A user generates a meal plan with a custom amount of calories
	Given the meal planner page is active
	When the add filters button is clicked
	And 1500 calories is entered
	And the generate meal plan button is clicked
	Then a meal plan with a total less then 4500 calories per day should be generated

Scenario: A user generates a meal plan with very healthy meals
	Given the meal planner page is active
	When the add filters button is clicked
	And the very healthy box is checked
	And the generate meal plan button is clicked
	Then a meal plan with a total less then 2000 calories per day should be generated

Scenario: A user wants to generate only dinner
	Given the meal planner page is active
	When the add filters button is clicked
	And only dinner is checked
	And the generate meal plan button is clicked
	Then a meal plan with only 7 dinners should be generated