using System.Collections.Generic;
using System.Linq;
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
                    throw new InvalidOperationException($"Informe quantas unidades há no pacote do ingrediente {ingredient.Name}.");

                decimal pricePerUnit = ingredient.PricePerUnit / ingredient.UnitsPerPackage;
                return pricePerUnit * quantity;
            }
            else {
                decimal weightInKg = UnitConverter.Convert(quantity, ingredient.Unit, UnitOfMeasure.Kilograms);
                var relevantStock = _stockItems.Where(s => s.Ingredient == ingredient).ToList();

                if (!relevantStock.Any())
                    throw new InvalidOperationException($"O ingrediente {ingredient.Name} não está registrado no estoque.");

                decimal totalCost = 0m;
                decimal remainingWeight = weightInKg;

                foreach (var stockItem in relevantStock.OrderBy(s => s.CalculateCostPerKilogram())) {
                    if (remainingWeight <= 0) break;

                    decimal weightToUse = remainingWeight > stockItem.WeightInKilograms
                        ? stockItem.WeightInKilograms
                        : remainingWeight;

                    totalCost += weightToUse * stockItem.CalculateCostPerKilogram();
                    remainingWeight -= weightToUse;
                }

                if (remainingWeight > 0)
                    throw new InvalidOperationException($"Estoque insuficiente para o ingrediente {ingredient.Name}.");

                return totalCost;
            }
        }

    }
}