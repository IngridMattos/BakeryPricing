using System.Collections.Generic;

namespace PrecificacaoConfeitaria.Domain.Entities {
    public class Recipe {
        public string Name { get; set; }
        public List<RecipeComponent> Components { get; set; } = new();
        public decimal TotalWeight => CalculateTotalWeight();
        public decimal TotalCost => CalculateTotalCost();

        public Recipe(string name) {
            Name = name;
        }

        public void AddComponent(RecipeComponent component) {
            Components.Add(component);
        }

        private decimal CalculateTotalWeight() {
            decimal totalWeight = 0;
            foreach (var component in Components) {
                totalWeight += component.WeightInKilograms;
            }
            return totalWeight;
        }

        private decimal CalculateTotalCost() {
            decimal totalCost = 0;
            foreach (var component in Components) {
                totalCost += component.CalculateCost();
            }
            return totalCost;
        }
    }
}
