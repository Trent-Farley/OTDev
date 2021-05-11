Feature: Move Shopping items
	After completing a shopping trip, add the items to your inventory with a single button.

Scenario: Add items to you inventory
	Given there are items to add
	When a user clicks the button
	Then the items are added to the inventory and removed from the shopping list.

Scenario: Want to clear old shopping list
	Given there are items
	When a user clicks the button
	Then all items are removed from the shopping list.