using PrecificacaoConfeitaria.Domain.Entities;

namespace PrecificacaoConfeitaria.Domain.Services {
    public class RecipePricingService {
        private readonly RecipePartCostService _partCostService;
        private readonly IndirectCostAllocator _allocator;

        public RecipePricingService(
            RecipePartCostService partCostService,
            IndirectCostAllocator allocator) {
            _partCostService = partCostService;
            _allocator = allocator;
        }

        public decimal Calculate(CustomRecipe recipe, decimal profitPercent) {
            decimal totalCost = 0;
            decimal totalKg = 0;

            foreach (var part in recipe.Parts) {
                totalCost += _partCostService.CalculatePartCost(part);

                totalKg += part.Ingredients.Sum(i => i.QuantityInKg);
            }

            totalCost += totalKg * _allocator.CostPerKg();

            var profit = totalCost * (profitPercent / 100m);
            return totalCost + profit;
        }

    }
}