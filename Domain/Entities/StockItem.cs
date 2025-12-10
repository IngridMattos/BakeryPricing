using PrecificacaoConfeitaria.Domain.Entities;

namespace PrecificacaoConfeitaria.Domain.Entities {
    public class StockItem {
        public Ingredient Ingredient { get; set; }
        public decimal WeightInKilograms { get; set; }
        public decimal TotalPrice { get; set; }

        public StockItem(Ingredient ingredient, decimal weightInKg, decimal totalPrice) {
            Ingredient = ingredient;
            WeightInKilograms = weightInKg;
            TotalPrice = totalPrice;
        }

        public decimal CalculateCostPerKilogram() {
            return TotalPrice / WeightInKilograms;
        }
    }
}