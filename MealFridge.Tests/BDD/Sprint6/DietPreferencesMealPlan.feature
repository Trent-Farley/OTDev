Feature: Dietary Preferences When Choosing Meals
	The user's diet is taken under consideration when selecting meal plan
	Link here: https://www.pivotaltracker.com/n/projects/2486697/stories/178102816

Scenario: A user generates meal plans
	Given a user generates meal plans
	When they have set a dietary restriction
	Then the meal plans generated will only contain compliant recipes


Scenario: A user sets a new dietary restriction
	Given a user wants to specify a new diet
	When they select a diet in the preference page
	Then the meal plans that are generated are compliant (only are shown) for the new diet
