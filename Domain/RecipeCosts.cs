using PrecificacaoConfeitaria.Domain.Entities;
using PrecificacaoConfeitaria.Domain.DomainServices;
using PrecificaçãoConfeitaria.Domain.Enums;

namespace PrecificaçãoConfeitaria.Domain {
    public class RecipeCosts {

            private UnitOfMeasure baseUnit = UnitOfMeasure.Grams;
        public decimal CalculateTotalCost(Recipe recipe) {
            decimal totalCost = 0m;
            foreach (var i in recipe.IngredientsAndQuantity) {

                totalCost += i.Ingredient.PricePerUnit * UnitConverter.Convert(i.Quantity, i.Unit, baseUnit);

            }
            return totalCost;
        }
    }
}
