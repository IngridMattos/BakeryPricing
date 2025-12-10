using PrecificacaoConfeitaria.Domain.Enums;

namespace PrecificacaoConfeitaria.Domain.Entities {
    public class StockItem {
        public Ingredient Ingredient { get; set; }
        public decimal WeightInKilograms { get; set; }
        public decimal PurchasePrice { get; set; } // Valor total pago pelo peso registrado

        public StockItem(Ingredient ingredient, decimal weightInKilograms, decimal purchasePrice) {
            Ingredient = ingredient;
            WeightInKilograms = weightInKilograms;
            PurchasePrice = purchasePrice;
        }

        public decimal CalculateCostPerKilogram() {
            return PurchasePrice / WeightInKilograms;
        }
    }
}