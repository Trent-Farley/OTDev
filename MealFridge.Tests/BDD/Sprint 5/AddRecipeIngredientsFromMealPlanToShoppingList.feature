Feature: Add Recipe Ingredients from Meal Planner to Shopping List
	Button that when pressed adds all the ingredients from the recipes in your meal plan to your shopping list. 

Scenario: A logged in user clicks the Add Recipes to Shopping List button
	Given the user is logged in
	When they click the button
	Then the button disapears
	And they are alerted of the updated shopping list
	And their shopping list is updated