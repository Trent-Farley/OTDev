Feature: Move Shopping items
	After completing a shopping trip, add the items to your inventory with a single button.
	https://www.pivotaltracker.com/story/show/177689561

Scenario: Add items from shopping list
	Given there are items clicked in the list
	When a user clicks the add button
	Then the items are added to the inventory and removed from the shopping list.

Scenario: Remove items from shopping list
	Given there are items clicked in the list
	When a user clicks the remove button
	Then all items are removed from the shopping list.