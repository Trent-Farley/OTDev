Feature: A logged in user would like to be able to filter a meal plan by characteristics of a recipe
	Link here: https://www.pivotaltracker.com/story/show/178102723
	Testing/Implementation done by: Trent Farley

Scenario Outline: A user generates a meal plan with custom filters
	Given a user is logged in
	And a user is on the meal planner page
	When a user adds advanced filters set to <calories>, <Fat>, <Protein>, <Cheap> then clicks generate meal plan
	Then a customized meal plan should be created

	# Second scenario outline with assertion for total to be less then provided filters
	Examples:
		| Calories | Fat | Protein | Cheap |
		| 2000     | 20  | 50      | true  |
		| 2000     | 80  | 10      | false |