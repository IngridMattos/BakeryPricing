using PrecificacaoConfeitaria.Domain.Entities;

namespace PrecificacaoConfeitaria.Domain.Services {
    public class RecipePricingService {
        private readonly RecipeCosts _recipeCosts;

        public RecipePricingService(StockService stockService) {
            _recipeCosts = new RecipeCosts(stockService);
        }

        public decimal CalculateFinalPrice(Recipe recipe, decimal profitPercentage) {
            var cost = _recipeCosts.CalculateTotalRecipeCost(recipe);
            recipe.TotalCost = cost;

            return cost * (1 + profitPercentage / 100m);
        }
    }
}