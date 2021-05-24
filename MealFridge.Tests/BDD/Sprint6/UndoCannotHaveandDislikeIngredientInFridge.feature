Feature: Undo Cannot Have and Dislike ingredients on the fridge page
	Toggle that allows the user to view recipes tagged as cheap on the recipe search page 

# Follow givens/whens/thens
# Given the user is on the fridge page
# And they removed an item
# When they click undo, the ingredient is unflagged and reappears in search
Scenario: A user clicks Cannot Have
	When the user clicks on cannot have
	Then the ingredient is removed from the search
	And the ingredient is flagged as cannot have
	And the user is alerted with a button
	When the user clicks undo
	Then the ingredient is unflagged
	And the ingredient reappears in the search

Scenario: A user clicks Dislike
	When the user clicks on Dislike
	Then the ingredient is removed from the search
	And the ingredient is flagged as Disliked
	And the user is alerted with a button
	When the user clicks undo
	Then the ingredient is unflagged
	And the ingredient reappears in the search