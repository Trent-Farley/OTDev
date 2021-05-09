using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace MealFridge.Models
{
    public partial class MealFridgeDbContext : DbContext
    {
        public MealFridgeDbContext()
        {
        }

        public MealFridgeDbContext(DbContextOptions<MealFridgeDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Diet> Diets { get; set; }
        public virtual DbSet<Fridge> Fridges { get; set; }
        public virtual DbSet<Ingredient> Ingredients { get; set; }
        public virtual DbSet<Meal> Meals { get; set; }
        public virtual DbSet<Recipe> Recipes { get; set; }
        public virtual DbSet<Recipeingred> Recipeingreds { get; set; }
        public virtual DbSet<Restriction> Restrictions { get; set; }
        public virtual DbSet<Savedrecipe> Savedrecipes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Name=MealFridge");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Diet>(entity =>
            {
                entity.HasKey(e => e.AccountId)
                    .HasName("PK__DIET__46A222CD81B2FD25");

                entity.ToTable("DIET");

                entity.Property(e => e.AccountId)
                    .HasMaxLength(255)
                    .HasColumnName("account_id");

                entity.Property(e => e.DairyFree).HasColumnName("dairy_free");

                entity.Property(e => e.GlutenFree).HasColumnName("gluten_free");

                entity.Property(e => e.Keto).HasColumnName("keto");

                entity.Property(e => e.LactoVeg).HasColumnName("lacto-veg");

                entity.Property(e => e.Metric).HasColumnName("metric");

                entity.Property(e => e.OvoVeg).HasColumnName("ovo-veg");

                entity.Property(e => e.Paleo).HasColumnName("paleo");

                entity.Property(e => e.Pescetarian).HasColumnName("pescetarian");

                entity.Property(e => e.Primal).HasColumnName("primal");

                entity.Property(e => e.Vegen).HasColumnName("vegen");

                entity.Property(e => e.Vegetarian).HasColumnName("vegetarian");

                entity.Property(e => e.Whole30).HasColumnName("whole30");
            });

            modelBuilder.Entity<Fridge>(entity =>
            {
                entity.HasKey(e => new { e.AccountId, e.IngredId })
                    .HasName("PK__FRIDGE__9100B6D1482DAB62");

                entity.ToTable("FRIDGE");

                entity.Property(e => e.AccountId)
                    .HasMaxLength(255)
                    .HasColumnName("account_id");

                entity.Property(e => e.IngredId).HasColumnName("ingred_id");

                entity.Property(e => e.NeededAmount).HasColumnName("needed_amount");

                entity.Property(e => e.Quantity).HasColumnName("quantity");

                entity.Property(e => e.Shopping).HasColumnName("shopping");

                entity.Property(e => e.UnitType)
                    .HasMaxLength(255)
                    .HasColumnName("unit_type");

                entity.HasOne(d => d.Ingred)
                    .WithMany(p => p.Fridges)
                    .HasForeignKey(d => d.IngredId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Fridge_FK_Ingred");
            });

            modelBuilder.Entity<Ingredient>(entity =>
            {
                entity.ToTable("INGREDIENTS");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.Aisle)
                    .HasMaxLength(255)
                    .HasColumnName("aisle");

                entity.Property(e => e.Calories).HasColumnName("calories");

                entity.Property(e => e.Carbs).HasColumnName("carbs");

                entity.Property(e => e.Cholesterol).HasColumnName("cholesterol");

                entity.Property(e => e.Cost)
                    .HasColumnType("money")
                    .HasColumnName("cost");

                entity.Property(e => e.Image)
                    .HasMaxLength(255)
                    .HasColumnName("image");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .HasColumnName("name");

                entity.Property(e => e.NetCarbs).HasColumnName("net_carbs");

                entity.Property(e => e.Protein).HasColumnName("protein");

                entity.Property(e => e.SatFat).HasColumnName("sat_fat");

                entity.Property(e => e.ServingSize).HasColumnName("serving_size");

                entity.Property(e => e.ServingUnit)
                    .HasMaxLength(255)
                    .HasColumnName("serving_unit");

                entity.Property(e => e.Sodium).HasColumnName("sodium");

                entity.Property(e => e.Sugar).HasColumnName("sugar");

                entity.Property(e => e.TotalFat).HasColumnName("total_fat");
            });

            modelBuilder.Entity<Meal>(entity =>
            {
                entity.HasKey(e => new { e.AccountId, e.Day })
                    .HasName("PK__MEAL__4B255BFF8A41DD46");

                entity.ToTable("MEAL");

                entity.Property(e => e.AccountId)
                    .HasMaxLength(255)
                    .HasColumnName("account_id");

                entity.Property(e => e.Day)
                    .HasColumnType("datetime")
                    .HasColumnName("day");

                entity.Property(e => e.MealType).HasMaxLength(255);

                entity.Property(e => e.RecipeId).HasColumnName("recipe_id");

                entity.HasOne(d => d.Recipe)
                    .WithMany(p => p.Meals)
                    .HasForeignKey(d => d.RecipeId)
                    .HasConstraintName("Meal_FK_Recipe");
            });

            modelBuilder.Entity<Recipe>(entity =>
            {
                entity.ToTable("RECIPE");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.Breakfast).HasColumnName("breakfast");

                entity.Property(e => e.Calories).HasColumnName("calories");

                entity.Property(e => e.Carbs).HasColumnName("carbs");

                entity.Property(e => e.Cheap).HasColumnName("cheap");

                entity.Property(e => e.Cholesterol).HasColumnName("cholesterol");

                entity.Property(e => e.Cost)
                    .HasColumnType("money")
                    .HasColumnName("cost");

                entity.Property(e => e.DairyFree).HasColumnName("dairy_free");

                entity.Property(e => e.Dessert).HasColumnName("dessert");

                entity.Property(e => e.Dinner).HasColumnName("dinner");

                entity.Property(e => e.GlutenFree).HasColumnName("gluten_free");

                entity.Property(e => e.Image)
                    .HasMaxLength(255)
                    .HasColumnName("image");

                entity.Property(e => e.Instructions)
                    .HasMaxLength(255)
                    .HasColumnName("instructions");

                entity.Property(e => e.Keto).HasColumnName("keto");

                entity.Property(e => e.LactoVeg).HasColumnName("lacto-veg");

                entity.Property(e => e.Lunch).HasColumnName("lunch");

                entity.Property(e => e.Minutes).HasColumnName("minutes");

                entity.Property(e => e.NetCarbs).HasColumnName("net_carbs");

                entity.Property(e => e.OvoVeg).HasColumnName("ovo-veg");

                entity.Property(e => e.Paleo).HasColumnName("paleo");

                entity.Property(e => e.Pescetarian).HasColumnName("pescetarian");

                entity.Property(e => e.Primal).HasColumnName("primal");

                entity.Property(e => e.Protein).HasColumnName("protein");

                entity.Property(e => e.SatFat).HasColumnName("sat_fat");

                entity.Property(e => e.ServingSize).HasColumnName("serving_size");

                entity.Property(e => e.ServingUnit)
                    .HasMaxLength(255)
                    .HasColumnName("serving_unit");

                entity.Property(e => e.Servings).HasColumnName("servings");

                entity.Property(e => e.Snack).HasColumnName("snack");

                entity.Property(e => e.Sodium).HasColumnName("sodium");

                entity.Property(e => e.Sugar).HasColumnName("sugar");

                entity.Property(e => e.Summery)
                    .HasMaxLength(255)
                    .HasColumnName("summery");

                entity.Property(e => e.Title)
                    .HasMaxLength(255)
                    .HasColumnName("title");

                entity.Property(e => e.TotalFat).HasColumnName("total_fat");

                entity.Property(e => e.Vegen).HasColumnName("vegen");

                entity.Property(e => e.Vegetarian).HasColumnName("vegetarian");

                entity.Property(e => e.VeryHealthy).HasColumnName("very_healthy");

                entity.Property(e => e.Whole30).HasColumnName("whole30");
            });

            modelBuilder.Entity<Recipeingred>(entity =>
            {
                entity.HasKey(e => new { e.RecipeId, e.IngredId })
                    .HasName("PK__RECIPEIN__E2D3798771FAABAF");

                entity.ToTable("RECIPEINGRED");

                entity.Property(e => e.RecipeId).HasColumnName("recipe_id");

                entity.Property(e => e.IngredId).HasColumnName("ingred_id");

                entity.Property(e => e.Amount).HasColumnName("amount");

                entity.Property(e => e.Calories).HasColumnName("calories");

                entity.Property(e => e.Carbs).HasColumnName("carbs");

                entity.Property(e => e.Cholesterol).HasColumnName("cholesterol");

                entity.Property(e => e.Direction)
                    .HasMaxLength(255)
                    .HasColumnName("direction");

                entity.Property(e => e.NetCarbs).HasColumnName("net_carbs");

                entity.Property(e => e.Protein).HasColumnName("protein");

                entity.Property(e => e.SatFat).HasColumnName("sat_fat");

                entity.Property(e => e.ServingUnit)
                    .HasMaxLength(255)
                    .HasColumnName("serving_unit");

                entity.Property(e => e.Sodium).HasColumnName("sodium");

                entity.Property(e => e.Sugar).HasColumnName("sugar");

                entity.Property(e => e.TotalFat).HasColumnName("total_fat");

                entity.HasOne(d => d.Ingred)
                    .WithMany(p => p.Recipeingreds)
                    .HasForeignKey(d => d.IngredId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("RecipeIngred_FK_Ingred");

                entity.HasOne(d => d.Recipe)
                    .WithMany(p => p.Recipeingreds)
                    .HasForeignKey(d => d.RecipeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("RecipeIngred_FK_Recipe");
            });

            modelBuilder.Entity<Restriction>(entity =>
            {
                entity.HasKey(e => new { e.AccountId, e.IngredId })
                    .HasName("PK__RESTRICT__9100B6D112ADB472");

                entity.ToTable("RESTRICTIONS");

                entity.Property(e => e.AccountId)
                    .HasMaxLength(255)
                    .HasColumnName("account_id");

                entity.Property(e => e.IngredId).HasColumnName("ingred_id");

                entity.Property(e => e.Banned).HasColumnName("banned");

                entity.Property(e => e.Dislike).HasColumnName("dislike");

                entity.HasOne(d => d.Ingred)
                    .WithMany(p => p.Restrictions)
                    .HasForeignKey(d => d.IngredId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Restrictions_FK_Ingred");
            });

            modelBuilder.Entity<Savedrecipe>(entity =>
            {
                entity.HasKey(e => new { e.AccountId, e.RecipeId })
                    .HasName("PK__SAVEDREC__E5F53C14BF094D7D");

                entity.ToTable("SAVEDRECIPES");

                entity.Property(e => e.AccountId)
                    .HasMaxLength(255)
                    .HasColumnName("account_id");

                entity.Property(e => e.RecipeId).HasColumnName("recipe_id");

                entity.Property(e => e.Favorited).HasColumnName("favorited");

                entity.Property(e => e.Shelved).HasColumnName("shelved");

                entity.HasOne(d => d.Recipe)
                    .WithMany(p => p.Savedrecipes)
                    .HasForeignKey(d => d.RecipeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("SavedRecipes_FK_Recipes");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
