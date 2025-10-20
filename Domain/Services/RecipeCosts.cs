using PrecificacaoConfeitaria.Domain.Entities;
using PrecificacaoConfeitaria.Domain.Services;
using PrecificacaoConfeitaria.Domain.Enums;

namespace PrecificacaoConfeitaria.Domain.Services
{
    public class RecipeCosts
    {
        private readonly UnitOfMeasure _baseUnit = UnitOfMeasure.Kilograms;

        public decimal CalculateTotalRecipeCost(Recipe recipe)
        {
            decimal totalRecipeCost = 0m;

            foreach (var item in recipe.IngredientsAndQuantity)
            {
                decimal cost = 0m;

                if (item.Unit == UnitOfMeasure.Units)
                {
                    if (item.Ingredient.UnitsPerPackage <= 0)
                        throw new InvalidOperationException($"Informe quantas unidades há no pacote do ingrediente {item.Ingredient.Name}.");

                    decimal pricePerSingleUnit = item.Ingredient.PricePerUnit / item.Ingredient.UnitsPerPackage;
                    cost = pricePerSingleUnit * item.Quantity;
                }
                else
                {
                    decimal weightInKg = UnitConverter.Convert(item.Quantity, item.Unit, _baseUnit);
                    cost = weightInKg * item.Ingredient.PricePerKilogram;
                }

                totalRecipeCost += cost;
            }

            return totalRecipeCost;
        }
    }
}
