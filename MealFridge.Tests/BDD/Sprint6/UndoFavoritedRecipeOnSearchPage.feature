@Search
Feature: Undo Favorite on Search Recipe Page
	A button in an alert that allows the user to undo a favorite

Scenario: A user undoes a favorite 
	Given A user is on the recipe search page
	When they search for a recipe
	When they favorite a recipe
	When they undo the favorite
	

Scenario: A user favorites a recipe and does not undo it
	Given a user searched for a recipe
	When they favorited a reciep
	Then they are alerted
	And they dismiss the alert
