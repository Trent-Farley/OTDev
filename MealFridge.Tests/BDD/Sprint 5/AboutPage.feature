Feature: About Section
	About Us view and the Get Started view

Scenario: User visits about us from the front page
	When they click the About Us
	Then they are brought to the About Us page

Scenario: User visits the About Us page from the Get Started page
	When they click to get more information on the project
	Then they are brought to the About Us Page

Scenario: First time user visits Get Started
	Given the user is not logged in
	When they click Get Started on the home page
	Then they see the Get Started page with an option to register an account

Scenario: Logged in user visits Get Started
	Given the user is logged in
	When they click Get Started on the home page
	Then they go to the Get Started page without an option to register an account