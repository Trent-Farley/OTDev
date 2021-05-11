Feature: Shopping Strikethrough
	Strikethrough items as you shop.

Scenario: Strike an item
	Given the checkbox is clicked
	When I have gotten the item
	Then the item is struckthrough

Scenario: Unstrike an item
	Given the checkbox is clicked
	And the item was struck through
	When I realized I forgot something
	Then the item should become unstruck