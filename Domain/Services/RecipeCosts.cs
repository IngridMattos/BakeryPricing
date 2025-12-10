using PrecificacaoConfeitaria.Domain.Entities;

namespace PrecificacaoConfeitaria.Domain.Services {
    public class RecipeCosts {
        private readonly StockService _stockService;

        public RecipeCosts(StockService stockService) {
            _stockService = stockService;
        }

        public decimal CalculateTotalRecipeCost(Recipe recipe, IndirectCostAllocator indirectAllocator) {
            decimal ingredientCost = 0m;

            foreach (var component in recipe.Components) {
                ingredientCost += _stockService.CalculateIngredientCost(component.Ingredient, component.Quantity);
            }

            decimal indirectCosts = recipe.CalculateIndirectCosts(indirectAllocator);

            return ingredientCost + indirectCosts;
        }
    }
}