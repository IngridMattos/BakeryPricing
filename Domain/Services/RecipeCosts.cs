using PrecificacaoConfeitaria.Domain.Entities;
using PrecificacaoConfeitaria.Domain.Enums;

namespace PrecificacaoConfeitaria.Domain.Services {
    public class RecipeCosts {
        private readonly UnitOfMeasure _baseUnit = UnitOfMeasure.Kilograms;
        private readonly StockService _stockService;

        public RecipeCosts(StockService stockService) {
            _stockService = stockService;
        }

        public decimal CalculateTotalRecipeCost(Recipe recipe) {
            var recipeCosts = new RecipeCosts(stockService);
            decimal totalRecipeCost = 0m;

            foreach (var item in recipe.Components) {
                decimal cost = _stockService.CalculateIngredientCost(item.Ingredient, item.Quantity);
                totalRecipeCost += cost;
            }

            return totalRecipeCost;
        }
    }
}
