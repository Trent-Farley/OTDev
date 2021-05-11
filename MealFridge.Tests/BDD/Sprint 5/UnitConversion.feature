Feature: Unit Converter
	"Simple" Unit Converter for converting between measurement types.
	https://www.pivotaltracker.com/story/show/177918449

Scenario: Convert a mass
	Given the amount is 16
	And the type is ounces
	And the convert is pounds
	When the the amount is converted
	Then the result should be 1

Scenario: Convert a volume
	Given the amount is 16
	And the type is cups
	And the convert is gallons
	When the the amount is converted
	Then the result should be 1