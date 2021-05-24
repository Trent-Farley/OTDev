Feature: Search for Recipes by Cuisine
	Search for food by Cuisines such as Greek, Italian, and Mexican.
	https://www.pivotaltracker.com/story/show/177917951

Scenario: A user wants to search for a cuisine
	Given A user is on the recipe search page
	When they select a single cuisine 
	Then recipes of that type should be the only result

Scenario: A user wants to exlude a Cuisine
	Given A user is on the recipe search page
	When They select a cuisine as an exclusion
	Then recipes of that type should not show.