@Inventory
Feature: Undo Cannot Have and Dislike ingredients on the fridge page
	Toggle that allows the user to view recipes tagged as cheap on the recipe search page 

# Follow givens/whens/thens
# Given the user is on the fridge page
# And they removed an item
# When they click undo, the ingredient is unflagged and reappears in search

Scenario: A user clicks Cannot Have 
	Given A user is on the inventory page
	When They search for an ingredient
	When they cannot have an ingredient
	When they undo it
	Then the ingredient should reappear in search

Scenario: A user clicks Cannot Have And Then Refreshes and searches again
	Given A user is on the inventory page
	When They search for an ingredient
	When they cannot have an ingredient
	When they undo it
	When they retry
	When They search for an ingredient
	Then the ingredient should reappear in search

Scenario: A user clicks Dislike
	Given A user is on the inventory page
	When They search for an ingredient
	When they dislike an ingredient
	When they undo it
	Then the ingredient should reappear in search

Scenario: A user clicks Dislike And Then Refreshes and searches again
	Given A user is on the inventory page
	When They search for an ingredient
	When they dislike an ingredient
	When they undo it
	When they retry
	When They search for an ingredient
	Then the ingredient should reappear in search