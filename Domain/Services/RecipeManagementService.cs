using System.Collections.Generic;
using System.Linq;
using PrecificacaoConfeitaria.Domain.Entities;

namespace PrecificacaoConfeitaria.Domain.Services {
    public class RecipeManagementService {
        private readonly List<Category> _categories = new();
        private readonly List<RecipeComponent> _recipeComponents = new();
        private readonly List<Recipe> _recipes = new();

        public void AddCategory(Category category) {
            _categories.Add(category);
        }

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
            Console.WriteLine("\n=== Receitas por Categoria ===");

            foreach (var category in _categories) {
                Console.WriteLine($"\nCategoria: {category.Name}");

                var componentsInCategory = _recipeComponents.Where(c => c.Category.Name == category.Name).ToList();
                if (componentsInCategory.Any()) {
                    Console.WriteLine("  Partes de Receita:");
                    foreach (var component in componentsInCategory) {
                        Console.WriteLine($"    - {component.Name} ({component.WeightInKilograms} kg, R$ {component.PricePerKilogram}/kg)");
                    }
                }

                var recipesInCategory = _recipes.Where(r => r.Components.Any(c => c.Category.Name == category.Name)).ToList();
                if (recipesInCategory.Any()) {
                    Console.WriteLine("  Receitas Completas:");
                    foreach (var recipe in recipesInCategory) {
                        Console.WriteLine($"    - {recipe.Name} (Peso: {recipe.TotalWeight} kg, Custo: R$ {recipe.TotalCost})");
                    }
                }
            }
        }
    }
}
