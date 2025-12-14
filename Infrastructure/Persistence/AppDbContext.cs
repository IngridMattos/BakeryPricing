using Microsoft.EntityFrameworkCore;
using PrecificacaoConfeitaria.Domain.Entities;
using System.Collections.Generic;
using System.Reflection.Emit;


namespace PrecificacaoConfeitaria.Infrastructure.Persistence {
    public class AppDbContext : DbContext {
        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) {
        }

        public DbSet<Ingredient> Ingredients => Set<Ingredient>();
        public DbSet<StockItem> StockItems => Set<StockItem>();
        public DbSet<RecipePart> RecipeParts => Set<RecipePart>();
        public DbSet<RecipePartIngredient> RecipePartIngredients => Set<RecipePartIngredient>();
        public DbSet<RecipePartType> RecipePartTypes => Set<RecipePartType>();
        public DbSet<CustomRecipe> CustomRecipes => Set<CustomRecipe>();
        public DbSet<IndirectCost> IndirectCosts => Set<IndirectCost>();

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);
        }
    }
}