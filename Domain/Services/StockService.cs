using PrecificacaoConfeitaria.Domain.Entities;
using System.Collections.Generic;
using System.Linq;

namespace PrecificacaoConfeitaria.Domain.Services {
    public class StockService {
        private readonly List<StockItem> _items = new();

        public void AddStockItem(StockItem item) {
            _items.Add(item);
        }

        public decimal GetCostPerKg(Ingredient ingredient) {
            var item = _items.First(i => i.Ingredient == ingredient);
            return item.TotalPrice / item.QuantityInKg;
        }
    }
}