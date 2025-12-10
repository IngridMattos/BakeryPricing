using PrecificacaoConfeitaria.Domain.Enums;
using PrecificacaoConfeitaria.Domain.Entities;

namespace PrecificacaoConfeitaria.Domain.Services {
    public class StockService {
        private readonly List<StockItem> _stockItems = new();

        public void AddStockItem(StockItem stockItem) {
            _stockItems.Add(stockItem);
        }

        public decimal CalculateIngredientCost(Ingredient ingredient, decimal quantity) {
            if (ingredient.Unit == UnitOfMeasure.Units) {
                if (ingredient.UnitsPerPackage <= 0)
                    throw new InvalidOperationException($"Informe unidades por pacote para {ingredient.Name}.");

                decimal pricePerUnit = ingredient.PricePerUnit / ingredient.UnitsPerPackage;
                return pricePerUnit * quantity;
            }

            decimal weightInKg =
                ingredient.Unit == UnitOfMeasure.Grams ?
                quantity / 1000m :
                quantity;

            var available = _stockItems.Where(s => s.Ingredient == ingredient).ToList();

            if (!available.Any())
                throw new InvalidOperationException($"Sem estoque de {ingredient.Name}");

            decimal totalCost = 0;
            decimal remaining = weightInKg;

            foreach (var stock in available.OrderBy(s => s.CalculateCostPerKilogram())) {
                if (remaining <= 0) break;

                decimal use = Math.Min(stock.WeightInKilograms, remaining);
                totalCost += use * stock.CalculateCostPerKilogram();
                remaining -= use;
            }

            if (remaining > 0)
                throw new InvalidOperationException($"Estoque insuficiente de {ingredient.Name}");

            return totalCost;
        }
    }
}