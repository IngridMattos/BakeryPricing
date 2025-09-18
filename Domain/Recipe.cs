namespace PrecificaçãoConfeitaria.Domain {
    public class Recipe {
        public string Name { get; set; }
        public List<Ingredient> Ingredients { get; set; } = new();
        public double Amount { get; set; }
        public TimeSpan PreparationTime { get; set; }

        public Recipe(string name, List<Ingredient> ingredients, double amount, TimeSpan preparationTime) {
            Name = name;
            Ingredients = ingredients;
            Amount = amount;
            PreparationTime = preparationTime;
        }

        public double TotalPriceRecipe() {

            double totalPrice = 0;

            foreach (var ingredient in Ingredients) { 
                totalPrice += ingredient.PricePerUnit * ingredient.UnitOfMeasure;
            }
            return totalPrice;

        } 

    }
}
