Feature: Meal planning with	an account's ingredient restrictions
	A logged in user can generate a meal plan that follows their ingredient restrictions
	Link here https://www.pivotaltracker.com/story/show/177916701 
	Testing/Implementation done by: Trent Farley

Scenario: A logged in user clicks generate meal plan
	Given a user is logged in and on the meal planning page
	When a user clicks the generate meal plan button
	Then a meal plan should generate with a flag warning the user if an ingredient is possibly disliked

Scenario: A logged in user clicks generate meal plan
	Given a user is logged in and on the meal planning page
	When a user clicks the generate meal plan button
	Then a meal plan should generate with meals that do not contain banned ingredients