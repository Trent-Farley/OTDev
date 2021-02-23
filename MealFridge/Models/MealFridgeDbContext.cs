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

        public virtual DbSet<Account> Accounts { get; set; }
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

            modelBuilder.Entity<Fridge>(entity =>
            {
                entity.HasKey(e => new { e.FridgeId, e.IngredId })
                    .HasName("PK__FRIDGE__FCA97214534B8678");

                entity.HasOne(d => d.FridgeNavigation)
                    .WithMany(p => p.Fridges)
                    .HasForeignKey(d => d.FridgeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Fridge_FK_Account");

                entity.HasOne(d => d.Ingred)
                    .WithMany(p => p.Fridges)
                    .HasForeignKey(d => d.IngredId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Fridge_FK_Ingred");
            });

            modelBuilder.Entity<Ingredient>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<Meal>(entity =>
            {
                entity.HasKey(e => new { e.AccountId, e.Day })
                    .HasName("PK__MEAL__4B255BFF8466FAE4");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Meals)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Meal_FK_Account");

                entity.HasOne(d => d.Recipe)
                    .WithMany(p => p.Meals)
                    .HasForeignKey(d => d.RecipeId)
                    .HasConstraintName("Meal_FK_Recipe");
            });

            modelBuilder.Entity<Recipe>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<Recipeingred>(entity =>
            {
                entity.HasKey(e => new { e.RecipeId, e.IngredId })
                    .HasName("PK__RECIPEIN__E2D37987FA6D531A");

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
                    .HasName("PK__RESTRICT__9100B6D1C7FC0541");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Restrictions)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Restrictions_FK_Account");

                entity.HasOne(d => d.Ingred)
                    .WithMany(p => p.Restrictions)
                    .HasForeignKey(d => d.IngredId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Restrictions_FK_Ingred");
            });

            modelBuilder.Entity<Savedrecipe>(entity =>
            {
                entity.HasKey(e => new { e.AccountId, e.RecipeId })
                    .HasName("PK__SAVEDREC__E5F53C1474F288FC");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Savedrecipes)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("SavedRecipes_FK_Account");

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
