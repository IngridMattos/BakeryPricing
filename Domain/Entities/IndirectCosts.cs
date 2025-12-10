namespace PrecificacaoConfeitaria.Domain.Entities {
    public class IndirectCost {
        public string Name { get; set; }
        public decimal Amount { get; set; }

        public IndirectCost(string name, decimal amount) {
            Name = name;
            Amount = amount;
        }
    }
}