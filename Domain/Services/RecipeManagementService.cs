using System;
using System.Collections.Generic;
using System.Linq;
using PrecificacaoConfeitaria.Domain.Entities;
using PrecificacaoConfeitaria.Domain.Enums;

namespace PrecificacaoConfeitaria.Domain.Services {
    public class RecipeManagementService {
        private readonly List<RecipeComponent> _recipeComponents = new();
        private readonly List<Recipe> _recipes = new();

        public void AddRecipeComponent(RecipeComponent component) {
            _recipeComponents.Add(component);
        }

        public void AddRecipe(Recipe recipe) {
            _recipes.Add(recipe);
        }

        public List<RecipeComponent> GetAllComponents() {
            return _recipeComponents;
        }

        public void ListAllRecipes() {
            Console.WriteLine("\n=== Receitas ===");

            foreach (var recipe in _recipes) {
                Console.WriteLine($"\n🍰 {recipe.Name}");
                Console.WriteLine($"Peso total: {recipe.TotalWeight} kg");
                Console.WriteLine($"Custo total: R$ {recipe.TotalCost}");

                foreach (var c in recipe.Components) {
                    Console.WriteLine($" - {c.Ingredient.Name}, {c.Quantity} ({c.Category})");
                }
            }
        }
    }
}
