using PrecificacaoConfeitaria.Domain.Entities;

namespace PrecificacaoConfeitaria.Domain.Services {
    public class RecipePartCostService {
        private readonly StockService _stockService;

        public RecipePartCostService(StockService stockService) {
            _stockService = stockService;
        }

        public decimal CalculatePartCost(RecipePart part) {
            decimal total = 0;

            foreach (var item in part.Ingredients) {
                var unitCost = _stockService.GetCostPerKg(item.Ingredient);
                total += unitCost * item.QuantityInKg;
            }

            return total;
        }
    }
}