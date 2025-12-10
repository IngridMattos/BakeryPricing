using System;
using System.Collections.Generic;
using PrecificacaoConfeitaria.Domain.Entities;
using PrecificacaoConfeitaria.Domain.Enums;

namespace PrecificacaoConfeitaria.Domain.Services {
    public class RecipeBuilder {
        private readonly List<Ingredient> _ingredients;

        public RecipeBuilder(List<Ingredient> ingredients) {
            _ingredients = ingredients;
        }

        public Recipe BuildRecipe(string recipeName, List<(string ingredientName, decimal quantity, RecipeComponentCategory category)> items) {
            var recipe = new Recipe(recipeName);

            foreach (var (ingredientName, quantity, category) in items) {
                var ingredient = _ingredients.Find(i => i.Name == ingredientName);

                if (ingredient == null)
                    throw new InvalidOperationException($"Ingrediente {ingredientName} não encontrado.");

                recipe.AddComponent(new RecipeComponent(ingredient, quantity, category));
            }

            return recipe;
        }
    }
}
