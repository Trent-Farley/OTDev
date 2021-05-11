Feature: Remove Common Ingredients From Inventory
    The user can see common ingredients between a searched recipe and their inventory and 
    delete the common ingredients.
    Link here: https://www.pivotaltracker.com/n/projects/2486697/stories/176855445

Scenario: A user selects a recipe's details in the search page
    Given a user presses the recipe details button
    When they are interested in the ingredients contained in the recipe
    Then they can also see common ingredients between their inventory and the recipe

Scenario: A user presses the cooked button on the recipe modal
    Given a user presses the cooked button
    When they are on the recipe modal page 
    Then the common ingredients quantity is reduced by a single amount


