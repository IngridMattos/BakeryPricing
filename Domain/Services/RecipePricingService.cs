using PrecificacaoConfeitaria.Domain.Entities;
using PrecificacaoConfeitaria.Domain.Enums;


namespace PrecificacaoConfeitaria.Domain.Services {

    public class RecipePricingService {

        private readonly RecipeCosts _recipeCosts; //Encapsulamento: _recipeCosts é privado, protegendo a lógica interna.
        //readonly garante que ela só será atribuída no construtor, impedindo alteração posterior — bom para imutabilidade e segurança de dados.

        public RecipePricingService() {
            _recipeCosts = new RecipeCosts();
        }

        public decimal CalculateFinalPrice(Recipe recipe, decimal profitPercentage) {
            if (profitPercentage < 0)
                throw new ArgumentException("A margem de lucro não pode ser negativa.");

            decimal cost = _recipeCosts.CalculateTotalRecipeCost(recipe); //Dependency Inversion

            decimal finalPrice = cost * (1 + profitPercentage / 100m);
            return finalPrice;
        }
    }
}

