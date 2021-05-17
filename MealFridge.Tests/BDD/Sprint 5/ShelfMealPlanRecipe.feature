Feature: Shelf Recipe From Meal Plan View
    The user can dislike (shelf) a recipe generated from the meal plan
    Link here: https://www.pivotaltracker.com/n/projects/2486697/stories/177916827

Scenario: A user shelves a recipe from the meal plan view
    Given a user wants to shelf a recipe
    When they press the shelf button on the meal modal
    Then the recipe is designated as "shelved"

Scenario: A user checks their shelved recipes
    Given a user selects their preferences and navigates to their disliked recipes
    When they have previously shelved a recipe from their meal plan
    Then they can visualize that recipe in the shelved column
