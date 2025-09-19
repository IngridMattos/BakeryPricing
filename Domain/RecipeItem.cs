using PrecificaçãoConfeitaria.Domain;
using PrecificaçãoConfeitaria.Domain.Enums;

namespace PrecificacaoConfeitaria.Domain.Entities {
    public class RecipeItem {
        public Ingredient Ingredient { get; set; }
        public decimal Quantity { get; set; }
        public UnitOfMeasure Unit { get; set; }

        public RecipeItem(Ingredient ingredient, decimal quantity, UnitOfMeasure unit) {
            Ingredient = ingredient;
            Quantity = quantity;
            Unit = unit;
        }
    }
}

