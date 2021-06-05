# MealFridge
## Team Name: On Track Development



## Project Description
<p>
    As a senior capstone project, the Recipe Finder system will provide individuals with what to make for a meal off of the items that are in their inventory. These recipes should also be searchable by name, type, or other specified filters. The system will store the current inventory as well as the individual's preferences. These preferences will include what the person dislikes as well as any dietary accommodations that must be made. If an inventory does not contain enough ingredients or the individual wants to make an entire menu for any amount of days a shopping list should automatically be generated. Unlike the current applications, our application will accommodate a menu, shopping list, and both types of searches (by inventory and by name). 
</p>
<br><br>
<br><br>
<br><br>

## Table of Contents
1. [Project Description](#MealFridge)
2. [Installation](#Installation-And-Usage)
3. [Contributing](#Contributing-Guidelines)
4. [Credits](#Credits)
5. [Team Mettings](#Meeting-Times)

<br><br>

## Installation And Usage
<strong><i>To install this project please:</i></strong>
1. Clone repository 
2. Run ```up.sql``` then ```seed.sql``` to have a mock database
3. Run ```dotnet build``` then ```dotnet run``` 

<br><br>

## Contributing Guidelines
### 1. Files
All files need to be CamelCase  
### 2. Folders
All Folders need to be Camel_Snake_Case
### 3. SQL IDs
All Ids will be named with the table in mind. For a product, an id would be called
```SQL
product_id
```
### 4. HTML/CSS Ids/Class names
For this project, all Ids/Classes will use [BEM](https://www.integralist.co.uk/posts/bem/#4), which will use Block/Element/Modifier style. Whenever possible, we will utilize the css framework, [Bootstrap](https://getbootstrap.com/), for it's responsiveness to screen sizes and uniform structure of pages. 

### 5. C# Model naming
All models will have the name of the object they are representing in CamelCase. If a model is representing more then one object, it will have the name(s) of the objects it is representing together i.e ProductData. If a model is intended for a specific view, it needs to be in a ViewMoldels folder (this folder should be an exception to the folders naming convention). Models inside of the ViewModels folder need to be named after the view they are targetting, i.e. IndexViewModel. 

### 6. Private and Public variables
Private members 
```C#
    private _db;
```
Public members
```C#
    public Db
```

### 7. Client side Typescript
For all TypeScript architecture, [this style guide](https://basarat.gitbook.io/typescript/styleguide) must be used


### 8. Git
* Use branches
* Commit often (don't feel like you need to finish a whole feature)
* Don't commit code that doesn't compile 
* Follow [Conventional Commits](https://www.conventionalcommits.org/en/v1.0.0/) for commit messages
* Do not commit any generated files (bin, obj)

<br><br>

## Credits
This project was developed by:
<table>
<tbody>
    <tr>
        <td>
            Chris Edwards
        </td>
        <td>
           <a href="https://www.linkedin.com/in/christopher-edwards-17aba1183/">Linkedin</a><br><a href="https://github.com/chrisedwou">Github</a>
        </td>
    </tr>
    <tr>
        <td>
            Christian Morris
        </td>
        <td>
           <a href="https://github.com/cmorris19">Github</a>
        </td>
    </tr>
    <tr>
        <td>
            Josh Utter
        </td>
        <td>
           <a href="https://www.linkedin.com/in/joshua-utter-0897401b3">Linkedin</a><br><a href="https://github.com/Jutter18">Github</a>
        </td>
    </tr>
    <tr>
        <td>
            Trent Farley
        </td>
        <td>
           <a href="https://www.linkedin.com/in/trentfarley/">Linkedin</a><br><a href="https://github.com/Trent-Farley">Github</a>
        </td>
    </tr>
</tbody>
</table>
<br><br>

## Meeting Times
Day  | Time
------------- | -------------
Monday  | 3-5 pm
Tuesday  | 12pm - 2pm, 2pm - 3pm Scot Meeting
Thursday | 12 - 2pm 
