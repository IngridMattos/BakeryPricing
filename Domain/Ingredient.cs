using PrecificaçãoConfeitaria.Domain.Enums;

namespace PrecificacaoConfeitaria.Domain.Entities {
    public class Ingredient {
        public string Name { get; set; }
        public decimal PricePerUnit { get; set; } // preço por unidade base
        public UnitOfMeasure IngredientUnitOfMeasure { get; set; }

        public Ingredient(string name, decimal pricePerUnit, UnitOfMeasure unitOfMeasure) {
            Name = name;
            PricePerUnit = pricePerUnit;
            IngredientUnitOfMeasure = unitOfMeasure;
        }
    }
}
