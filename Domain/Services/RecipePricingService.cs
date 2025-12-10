using PrecificacaoConfeitaria.Domain.Entities;
using PrecificacaoConfeitaria.Domain.Enums;


namespace PrecificacaoConfeitaria.Domain.Services {

    public class RecipePricingService {

        private readonly RecipeCosts _recipeCosts;

        public RecipePricingService(StockService stockService) {
            _recipeCosts = new RecipeCosts(stockService);
            var recipeComponent = new RecipeComponent(recipeName, RecipeComponentCategory.Massa, recipePricePerKg, recipeWeight);
        }

        public decimal CalculateFinalPrice(Recipe recipe, decimal profitPercentage) {
            if (profitPercentage < 0)
                throw new ArgumentException("A margem de lucro não pode ser negativa.");

            decimal cost = _recipeCosts.CalculateTotalRecipeCost(recipe); //Dependency Inversion

            decimal finalPrice = cost * (1 + profitPercentage / 100m);
            return finalPrice;
        }
    }
}

