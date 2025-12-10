using System.Linq;
using PrecificacaoConfeitaria.Domain.Services;


namespace PrecificacaoConfeitaria.Domain.Entities {
    public class Recipe {
        public string Name { get; set; }
        public List<RecipeComponent> Components { get; set; } = new();

        public Recipe(string name) {
            Name = name;
        }

        public decimal GetTotalWeightInKg() {
            return Components.Sum(c => c.WeightInKilograms);
        }

        public decimal CalculateIndirectCosts(IndirectCostAllocator allocator) {
            return allocator.AllocateToRecipe(GetTotalWeightInKg());
        }
    }
}