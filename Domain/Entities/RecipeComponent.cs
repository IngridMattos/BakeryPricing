using PrecificacaoConfeitaria.Domain.Enums;

namespace PrecificacaoConfeitaria.Domain.Entities {
    public class RecipeComponent {
        public Ingredient Ingredient { get; set; }
        public decimal Quantity { get; set; } // em unidade definida pelo Ingredient
        public RecipeComponentCategory Category { get; set; }

        public RecipeComponent(Ingredient ingredient, decimal quantity, RecipeComponentCategory category) {
            Ingredient = ingredient;
            Quantity = quantity;
            Category = category;
        }

        public decimal WeightInKilograms =>
            Ingredient.Unit == UnitOfMeasure.Kilograms ?
                Quantity :
            Ingredient.Unit == UnitOfMeasure.Grams ?
                Quantity / 1000m :
                0; // produtos unitários não têm peso automático

        public decimal CalculateCost(decimal costPerKgFromStock) {
            return WeightInKilograms * costPerKgFromStock;
        }
    }
}