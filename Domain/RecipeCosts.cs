namespace PrecificaçãoConfeitaria.Domain {
    public class RecipeCosts {
        public Ingredient Ingredient { get; set; }
        public Recipe Recipe { get; set; }
        public double Quantity { get; set; }
        public RecipeCosts(Ingredient ingredient, Recipe recipe, double quantity) {
            Ingredient = ingredient;
            Recipe = recipe;
            Quantity = quantity;
        }
    }
}
