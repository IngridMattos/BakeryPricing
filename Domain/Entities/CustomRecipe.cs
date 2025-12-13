using System.Collections.Generic;

namespace PrecificacaoConfeitaria.Domain.Entities
{
    public class CustomRecipe {
        public string ClientName { get; }
        public List<RecipePart> Parts { get; } = new();

        public CustomRecipe(string clientName) {
            ClientName = clientName;
        }

        public void AddPart(RecipePart part) {
            Parts.Add(part);
        }
    }
}
