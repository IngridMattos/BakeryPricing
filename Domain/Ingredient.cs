using PrecificaçãoConfeitaria.Domain.Enums;

namespace PrecificacaoConfeitaria.Domain.Entities{
    public class Ingredient
    {
        public string Name { get; set; }
        public decimal PricePerKilogram { get; set; }
        public decimal PricePerUnit { get; set; }
        public UnitOfMeasure Unit { get; set; }

        public Ingredient(string name, decimal pricePerKilogram, UnitOfMeasure unit = UnitOfMeasure.Kilograms, decimal pricePerUnit = 0)
        {
            Name = name;
            PricePerKilogram = pricePerKilogram;
            Unit = unit;
            PricePerUnit = pricePerUnit;
        }
    }

}