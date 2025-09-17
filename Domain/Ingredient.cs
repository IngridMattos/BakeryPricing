using System.Runtime.ConstrainedExecution;

namespace PrecificaçãoConfeitaria.Domain {
    public class Ingredient {
        public string Name { get; set; }
        public double PurchasePrice { get; set; }
        public string UnitOfMeasure { get; set; }
        public Ingredient(string name, double purchasePrice, string unitOfMeasure) {
            Name = name;
            PurchasePrice = purchasePrice;
            UnitOfMeasure = unitOfMeasure;
        }
    }
}
