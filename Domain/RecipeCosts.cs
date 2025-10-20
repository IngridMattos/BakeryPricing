using PrecificacaoConfeitaria.Domain.Entities;
using PrecificacaoConfeitaria.Domain.DomainServices;
using PrecificaçãoConfeitaria.Domain.Enums;

namespace PrecificacaoConfeitaria.Domain.Entities
{
    public class RecipeCosts
    {
        private UnitOfMeasure baseUnit = UnitOfMeasure.Kilograms;

        public decimal CalculateTotalRecipeCost(Recipe recipe)
        {
            decimal totalRecipeCost = 0m;

            foreach (var item in recipe.IngredientsAndQuantity)
            {
                if (item.Unit == UnitOfMeasure.Units)
                {
                    decimal pricePerSingleUnit = item.Ingredient.PricePerUnit / item.Ingredient.UnitsPerPackage;
                    totalRecipeCost += pricePerSingleUnit * item.Quantity;
                }
                else
                {
                    decimal weightInKg = UnitConverter.Convert(item.Quantity, item.Unit, UnitOfMeasure.Kilograms);
                    totalRecipeCost += weightInKg * item.Ingredient.PricePerKilogram;
                }
            }

            return totalRecipeCost;
        }
    }
}
