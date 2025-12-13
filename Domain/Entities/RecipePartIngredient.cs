namespace PrecificacaoConfeitaria.Domain.Entities {
    public class RecipePartIngredient {
        public Ingredient Ingredient { get; }
        public decimal QuantityInKg { get; }

        public RecipePartIngredient(Ingredient ingredient, decimal quantityInKg) {
            Ingredient = ingredient;
            QuantityInKg = quantityInKg;
        }
    }
}