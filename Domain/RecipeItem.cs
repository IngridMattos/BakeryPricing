using PrecificaçãoConfeitaria.Domain;
using PrecificaçãoConfeitaria.Domain.Enums;

namespace PrecificacaoConfeitaria.Domain.Entities{
    public class RecipeItem {
        public Ingredient Ingredient { get; set; }
        public decimal Quantity { get; set; } // quantidade do ingrediente na receita
        public UnitOfMeasure Unit { get; set; } // unidade de medida da quantidade do ingrediente na receita

        public RecipeItem(Ingredient ingredient, decimal quantity, UnitOfMeasure unit) {
            Ingredient = ingredient;
            Quantity = quantity;
            Unit = unit;
        }
    }
}

