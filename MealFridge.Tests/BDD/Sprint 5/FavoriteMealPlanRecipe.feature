Feature: Favorite Recipe From Meal Plan View
    The user can favorite a recipe generated from the meal plan 
    Link here: https://www.pivotaltracker.com/n/projects/2486697/stories/177916820

Scenario: A user favorites a recipe from the meal plan view
    Given a user wants to favorite a recipe, or add it to their favorites
    When they select the favorite button on the meal modal
    Then the recipe is designated as "favorited"

Scenario: A user checks their favorited recipes
    Given a user selects their preferences and navigates to their favorited recipes
    When they have previously favorited a recipe from their meal plan
    Then they can visualize that recipe in the favorited column
