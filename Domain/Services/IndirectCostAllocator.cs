using PrecificacaoConfeitaria.Domain.Entities;

namespace PrecificacaoConfeitaria.Domain.Services {
    public class IndirectCostAllocator {
        private readonly IEnumerable<IndirectCost> _costs;
        private readonly decimal _monthlyKg;

        public IndirectCostAllocator(IEnumerable<IndirectCost> costs, decimal monthlyKg) {
            _costs = costs;
            _monthlyKg = monthlyKg;
        }

        public decimal CostPerKg() {
            if (_monthlyKg == 0) return 0;
            return _costs.Sum(c => c.Amount) / _monthlyKg;
        }
    }
}
