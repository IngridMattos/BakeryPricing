using System;
using System.Collections.Generic;
using PrecificaçãoConfeitaria.Domain.Enums;

namespace PrecificacaoConfeitaria.Domain.Entities{
    public class Recipe {
        public string Name { get; set; }
        public List<RecipeItem> IngredientsAndQuantity { get; set; } = new();
        public decimal Amount { get; set; } // quantidade final da receita
        public TimeSpan PreparationTime { get; set; }

        public Recipe(string name, List<RecipeItem> ingredientsAndQuantity) {
            Name = name;
            IngredientsAndQuantity = ingredientsAndQuantity;
            //Amount = amount;
            //PreparationTime = preparationTime;
        }
    }
}
