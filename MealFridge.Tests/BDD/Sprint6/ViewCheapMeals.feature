Feature: View Cheap Meals On Recipe Search Page
	Toggle that allows the user to view recipes tagged as cheap on the recipe search page 

Scenario: A user searches for cheap meals 
	When they search for  a recipe
	Then the results show recipes ordered by price
	And the results only include cheap meals

Scenario: A users searches with cheap meals disabled
	When they search for a recipe
	Then the results are not ordered by price
	And the results includes cheap meals and non-cheap meals
