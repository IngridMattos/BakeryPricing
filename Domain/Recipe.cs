using System;
using System.Collections.Generic;

namespace PrecificacaoConfeitaria.Domain.Entities {
    public class Recipe {
        public string Name { get; set; }
        public List<RecipeItem> IngredientsAndQuantity { get; set; } = new();
        public decimal Amount { get; set; } // quantidade final da receita
        public TimeSpan PreparationTime { get; set; }

        public Recipe(string name, decimal amount, TimeSpan preparationTime) {
            Name = name;
            Amount = amount;
            PreparationTime = preparationTime;
        }
    }
}
