Feature: Dietary Preferences When Searching For Recipes
	The user's diet is taken under consideration when searching for recipes
	Link here: https://www.pivotaltracker.com/n/projects/2486697/stories/178102872

Scenario: A user searches for recipes
	Given a user searches for a recipe or recipes
	When they have set a dietary restriction
	Then the search results will only contain compliant recipes

Scenario: A user sets a new dietary restriction
	Given a user wants to specify a new diet
	When they select a diet in the preference page
	Then the search recipe results are compliant (only are shown) for the new diet
