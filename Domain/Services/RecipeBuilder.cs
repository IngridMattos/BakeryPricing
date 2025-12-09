using PrecificacaoConfeitaria.Domain.Entities;

namespace PrecificacaoConfeitaria.Domain.Services {
    public class RecipeBuilder {
        private readonly List<RecipeComponent> _availableComponents;

        public RecipeBuilder(List<RecipeComponent> availableComponents) {
            _availableComponents = availableComponents;
        }

        public Recipe BuildRecipe(string recipeName, List<(string componentName, decimal weight)> selectedComponents) {
            var recipe = new Recipe(recipeName);

            foreach (var (componentName, weight) in selectedComponents) {
                var component = _availableComponents.Find(c => c.Name == componentName);
                if (component == null)
                    throw new InvalidOperationException($"Componente {componentName} não encontrado.");

                var adjustedComponent = new RecipeComponent(
                    component.Name,
                    component.Category,
                    component.PricePerKilogram,
                     weight
                );

                recipe.AddComponent(adjustedComponent);
            }

            return recipe;
        }
    }
}