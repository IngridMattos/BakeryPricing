using PrecificacaoConfeitaria.Domain.Entities;

namespace PrecificacaoConfeitaria.Domain.Services {
    public class IndirectCostAllocator {
        private readonly IEnumerable<IndirectCost> _indirectCosts;
        private readonly decimal _totalKilogramsProducedInMonth;

        public IndirectCostAllocator(IEnumerable<IndirectCost> indirectCosts, decimal totalKilogramsProducedInMonth) {
            _indirectCosts = indirectCosts;
            _totalKilogramsProducedInMonth = totalKilogramsProducedInMonth;
        }

        public decimal GetIndirectCostPerKg() {
            var totalIndirectCosts = _indirectCosts.Sum(c => c.Amount);
            if (_totalKilogramsProducedInMonth == 0) return 0;
            return totalIndirectCosts / _totalKilogramsProducedInMonth;
        }

        public decimal AllocateToRecipe(decimal recipeTotalWeightKg) {
            var costPerKg = GetIndirectCostPerKg();
            return recipeTotalWeightKg * costPerKg;
        }
    }
}