using PrecificacaoConfeitaria.Domain.Enums;

namespace PrecificacaoConfeitaria.Domain.Entities {
    public class Ingredient {
        public string Name { get; }
        public IngredientMeasureType MeasureType { get; }

        public Ingredient(string name, IngredientMeasureType measureType) {
            Name = name;
            MeasureType = measureType;
        }
    }
}