using System.Runtime.ConstrainedExecution;

namespace PrecificaçãoConfeitaria.Domain {
    public class Ingredient {
        public string Name { get; set; }
        public double PricePerUnit { get; set; }
        public UnitOfMeasure IngredientUnitOfMeasure { get; set; }
        public Ingredient(string name, double pricePerUnit, UnitOfMeasure unitOfMeasure) {
            Name = name;
            PricePerUnit = pricePerUnit;
            IngredientUnitOfMeasure = unitOfMeasure;
        }

        public enum UnitOfMeasure {
            Grams,
            Kilograms,
            Milliliters,
            Liters,
            Units
        }

    }
}
