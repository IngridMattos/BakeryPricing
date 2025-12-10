using PrecificacaoConfeitaria.Domain.Enums;

namespace PrecificacaoConfeitaria.Domain.Entities {
    public class RecipeComponent {
        public string Name { get; set; }
        public RecipeComponentCategory Category { get; set; }
        public decimal PricePerKilogram { get; set; }
        public decimal WeightInKilograms { get; set; }

        public RecipeComponent(string name, RecipeComponentCategory category, decimal pricePerKilogram, decimal weightInKilograms) {
            Name = name;
            Category = category;
            var componentsInCategory = _recipeComponents.Where(c => c.Category.Name == category.Name).ToList();
            var recipesInCategory = _recipes.Where(r => r.Components.Any(c => c.Category.Name == category.Name)).ToList();
            PricePerKilogram = pricePerKilogram;
            WeightInKilograms = weightInKilograms;
        }

        public decimal CalculateCost() {
            return PricePerKilogram * WeightInKilograms;
        }
    }
}
