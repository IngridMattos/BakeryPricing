using System.Collections.Generic;

namespace PrecificacaoConfeitaria.Domain.Entities {
    public class RecipePart {
        public string Name { get; }
        public RecipePartType Type { get; }
        public List<RecipePartIngredient> Ingredients { get; } = new();

        public RecipePart(string name, RecipePartType type) {
            Name = name;
            Type = type;
        }

        public void AddIngredient(RecipePartIngredient ingredient) {
            Ingredients.Add(ingredient);
        }
    }
}
