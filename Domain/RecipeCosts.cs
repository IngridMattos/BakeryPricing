using PrecificacaoConfeitaria.Domain.Entities;
using PrecificacaoConfeitaria.Domain.DomainServices;
using PrecificaçãoConfeitaria.Domain.Enums;

namespace PrecificacaoConfeitaria.Domain.Entities{
    public class RecipeCosts {

        private UnitOfMeasure baseUnit = UnitOfMeasure.Grams;

        public decimal CalculateTotalRecipeCost(Recipe recipe) {
            decimal totalRecipeCost = 0m;
            foreach (var i in recipe.IngredientsAndQuantity) {

                totalRecipeCost += i.Ingredient.PricePerUnit * UnitConverter.Convert(i.Quantity, i.Unit, baseUnit);

            }
            return totalRecipeCost;
        }
    }
}
