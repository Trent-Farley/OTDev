
CREATE TABLE [FRIDGE] (
  [id] int,
  [account_id] NVARCHAR(255),
  [ingred_id] int,
  [quantity] int,
  PRIMARY KEY ([account_id], [ingred_id])
)


CREATE TABLE [INGREDIENTS] (
  [id] int PRIMARY KEY,
  [image] nvarchar(255),
  [name] nvarchar(255),
  [aisle] nvarchar(255),
  [cost] money,
  [serving_size] float,
  [serving_unit] nvarchar(255),
  [calories] float,
  [total_fat] float,
  [sat_fat] float,
  [carbs] float,
  [net_carbs] float,
  [sugar] float,
  [cholesterol] float,
  [sodium] float,
  [protein] float
)


CREATE TABLE [RESTRICTIONS] (
  [account_id] NVARCHAR(255),
  [ingred_id] int,
  [dislike] bit,
  PRIMARY KEY ([account_id], [ingred_id])
)


CREATE TABLE [RECIPES] (
  [id] int PRIMARY KEY,
  [title] nvarchar(255),
  [image] nvarchar(255),
  [servings] int,
  [minutes] int,
  [summery] nvarchar(255),
  [instructions] nvarchar(255),
  [cost] money,
  [serving_size] float,
  [serving_unit] nvarchar(255),
  [calories] float,
  [total_fat] float,
  [sat_fat] float,
  [carbs] float,
  [net_carbs] float,
  [sugar] float,
  [cholesterol] float,
  [sodium] float,
  [protein] float
)


CREATE TABLE [RECIPEINGRED] (
  [recipe_id] int,
  [ingred_id] int,
  [amount] float,
  [direction] nvarchar(255),
  [serving_unit] nvarchar(255),
  [calories] float,
  [total_fat] float,
  [sat_fat] float,
  [carbs] float,
  [net_carbs] float,
  [sugar] float,
  [cholesterol] float,
  [sodium] float,
  [protein] float
  PRIMARY KEY ([recipe_id], [ingred_id])
)


CREATE TABLE [SAVEDRECIPES] (
  [account_id] NVARCHAR(255),
  [recipe_id] int,
  [shelved] bit,
  PRIMARY KEY ([account_id], [recipe_id])
)


CREATE TABLE [MEAL] (
  [account_id] NVARCHAR(255),
  [day] datetime,
  [recipe_id] int,
  [meal] nvarchar(255),
  PRIMARY KEY ([account_id], [day])
)


ALTER TABLE [FRIDGE] ADD CONSTRAINT [Fridge_FK_Ingred] FOREIGN KEY ([ingred_id]) REFERENCES [INGREDIENTS] ([id])


ALTER TABLE [RESTRICTIONS] ADD CONSTRAINT [Restrictions_FK_Ingred] FOREIGN KEY ([ingred_id]) REFERENCES [INGREDIENTS] ([id])


ALTER TABLE [RECIPEINGRED] ADD CONSTRAINT [RecipeIngred_FK_Recipe] FOREIGN KEY ([recipe_id]) REFERENCES [RECIPES] ([id])


ALTER TABLE [RECIPEINGRED] ADD CONSTRAINT [RecipeIngred_FK_Ingred] FOREIGN KEY ([ingred_id]) REFERENCES [INGREDIENTS] ([id])


ALTER TABLE [SAVEDRECIPES] ADD CONSTRAINT [SavedRecipes_FK_Recipes] FOREIGN KEY ([recipe_id]) REFERENCES [RECIPES] ([id])


ALTER TABLE [MEAL] ADD CONSTRAINT [Meal_FK_Recipe] FOREIGN KEY ([recipe_id]) REFERENCES [RECIPES] ([id])
