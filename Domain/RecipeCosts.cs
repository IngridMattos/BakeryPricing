using PrecificacaoConfeitaria.Domain.Entities;
using PrecificacaoConfeitaria.Domain.DomainServices;
using PrecificaçãoConfeitaria.Domain.Enums;

namespace PrecificacaoConfeitaria.Domain.Entities{
    public class RecipeCosts
    {
        private UnitOfMeasure baseUnit = UnitOfMeasure.Kilograms;

        public decimal CalculateTotalRecipeCost(Recipe recipe)
        {
            decimal totalRecipeCost = 0m;

            foreach (var item in recipe.IngredientsAndQuantity)
            {
                decimal weightInKg;

                if (item.Unit == UnitOfMeasure.Units)
                {
                    weightInKg = UnitConverter.Convert(item.Quantity, UnitOfMeasure.Units, baseUnit, item.Ingredient.PricePerUnit);
                }
                else
                {
                    weightInKg = UnitConverter.Convert(item.Quantity, item.Unit, baseUnit);
                }

                totalRecipeCost += item.Ingredient.PricePerKilogram * weightInKg;
            }

            return totalRecipeCost;
        }
    }

}
