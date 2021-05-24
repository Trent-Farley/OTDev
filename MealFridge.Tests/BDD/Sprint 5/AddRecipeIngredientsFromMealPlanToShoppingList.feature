Feature: Add Recipe Ingredients from Meal Planner to Shopping List
	Button that when pressed adds all the ingredients from the recipes in your meal plan to your shopping list. 

# Line 6= How will you know which button to click? i.e. meal plan to shopping list button
# Think about instead of what they are doing physically, but what is occuring in the system.
Scenario: A logged in user clicks the Add Recipes to Shopping List button
	Given the user is logged in
	When they click the button
	Then the button disapears
	And they are alerted of the updated shopping list
	And their shopping list is updated