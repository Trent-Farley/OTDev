
CREATE TABLE [FRIDGE] (
  [id] int,
  [account_id] NVARCHAR(255),
  [ingred_id] int,
  [quantity] int,
  PRIMARY KEY ([account_id], [ingred_id])

)
GO

CREATE TABLE [INGREDIENTS] (
  [id] int PRIMARY KEY,
  [name] nvarchar(255),
  [aisle] nvarchar(255),
  [cost] money
)
GO

CREATE TABLE [RESTRICTIONS] (
  [account_id] NVARCHAR(255),
  [ingred_id] int,
  [dislike] bit,
  PRIMARY KEY ([account_id], [ingred_id])
)
GO

CREATE TABLE [RECIPES] (
  [id] int PRIMARY KEY,
  [title] nvarchar(255),
  [image] nvarchar(255),
  [servings] int,
  [minutes] int,
  [summery] nvarchar(255),
  [instructions] nvarchar(255)
)
GO

CREATE TABLE [RECIPEINGRED] (
  [recipe_id] int,
  [ingred_id] int,
  [amount] nvarchar(255),
  [step] int,
  [direction] nvarchar(255),
  PRIMARY KEY ([recipe_id], [ingred_id])
)
GO

CREATE TABLE [SAVEDRECIPES] (
  [account_id] NVARCHAR(255),
  [recipe_id] int,
  [shelved] bit,
  PRIMARY KEY ([account_id], [recipe_id])
)
GO

CREATE TABLE [MEAL] (
  [account_id] NVARCHAR(255),
  [account_id] int,
  [day] datetime,
  [recipe_id] int,
  [meal] nvarchar(255),
  PRIMARY KEY ([account_id], [day])
)
GO

ALTER TABLE [FRIDGE] ADD CONSTRAINT [Fridge_FK_Ingred] FOREIGN KEY ([ingred_id]) REFERENCES [INGREDIENTS] ([id])
GO

ALTER TABLE [RESTRICTIONS] ADD CONSTRAINT [Restrictions_FK_Ingred] FOREIGN KEY ([ingred_id]) REFERENCES [INGREDIENTS] ([id])
GO

ALTER TABLE [RECIPEINGRED] ADD CONSTRAINT [RecipeIngred_FK_Recipe] FOREIGN KEY ([recipe_id]) REFERENCES [RECIPES] ([id])
GO

ALTER TABLE [RECIPEINGRED] ADD CONSTRAINT [RecipeIngred_FK_Ingred] FOREIGN KEY ([ingred_id]) REFERENCES [INGREDIENTS] ([id])
GO

ALTER TABLE [SAVEDRECIPES] ADD CONSTRAINT [SavedRecipes_FK_Recipes] FOREIGN KEY ([recipe_id]) REFERENCES [RECIPES] ([id])
GO

ALTER TABLE [MEAL] ADD CONSTRAINT [Meal_FK_Recipe] FOREIGN KEY ([recipe_id]) REFERENCES [RECIPES] ([id])
GO