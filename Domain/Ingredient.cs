using PrecificaçãoConfeitaria.Domain.Enums;

namespace PrecificacaoConfeitaria.Domain.Entities{
    public class Ingredient {
        public string Name { get; set; }
        public decimal PricePerKilogram { get; set; } // preço sempre por kg

        public Ingredient(string name, decimal pricePerKilogram) {
            Name = name;
            PricePerKilogram = pricePerKilogram;
        }
    }
}
