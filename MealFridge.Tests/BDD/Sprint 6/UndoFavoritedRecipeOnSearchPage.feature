Feature: Undo Favorite on Search Recipe Page
	A button in an alert that allows the user to undo a favorite

Scenario: A user undos a favorite 
	Given the user searched for a recipe
	When they favorite a recipe
	Then they are alerted
	And they click the undo button
	Then the recipe is no longer favorited

Scenario: A user favorites a recipe and does not undo it
	Given a user searched for a recipe
	When they favorited a reciep
	Then they are alerted
	And they dismiss the alert
