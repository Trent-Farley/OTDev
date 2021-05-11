Feature: Added to Shopping list Alert
	Notification that can take you to your shopping list.
	https://www.pivotaltracker.com/story/show/177917759

Scenario: A user adds a recipe to their shopping list
	Given A user wants to add a recipe to their shopping list
	When They click the add to shopping list button
	Then An alert should display

Scenario: A user uses the alert to view their shopping list
	Given the alert pops
	When the user clicks the shopping link
	Then they are taken to their shopping list