Feature: Search for Recipes by Cuisine
	Search for food by Cuisines such as Greek, Italian, and Mexican.

Scenario: A user wants to search for a cuisine
	Given A user is on the recipe search page
	And they have search cuisine selected
	When They enter the cuisine 
	Then Recipes of that type should result

Scenario: A user wants to exlude a Cuisine
	Given A user is on the recipe search page
	And they have search cuisine selected
	When They enter the cuisine as an exclusion
	Then Recipes of that type should not show.