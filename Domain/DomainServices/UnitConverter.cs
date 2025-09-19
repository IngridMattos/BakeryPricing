using PrecificaçãoConfeitaria.Domain.Enums;

namespace PrecificacaoConfeitaria.Domain.DomainServices {
    public static class UnitConverter {
        public static decimal Convert(decimal quantity, UnitOfMeasure from, UnitOfMeasure to) {
            if (from == to)
                return quantity;

            switch (from) {
                case UnitOfMeasure.Grams:
                    if (to == UnitOfMeasure.Kilograms) return quantity / 1000m;
                    if (to == UnitOfMeasure.Milliliters) return quantity;   // assumindo densidade = 1
                    if (to == UnitOfMeasure.Liters) return quantity / 1000m;
                    break;

                case UnitOfMeasure.Kilograms:
                    if (to == UnitOfMeasure.Grams) return quantity * 1000m;
                    if (to == UnitOfMeasure.Milliliters) return quantity * 1000m;
                    if (to == UnitOfMeasure.Liters) return quantity;
                    break;

                case UnitOfMeasure.Milliliters:
                    if (to == UnitOfMeasure.Liters) return quantity / 1000m;
                    if (to == UnitOfMeasure.Grams) return quantity;
                    if (to == UnitOfMeasure.Kilograms) return quantity / 1000m;
                    break;

                case UnitOfMeasure.Liters:
                    if (to == UnitOfMeasure.Milliliters) return quantity * 1000m;
                    if (to == UnitOfMeasure.Grams) return quantity * 1000m;
                    if (to == UnitOfMeasure.Kilograms) return quantity;
                    break;

                case UnitOfMeasure.Units:
                    throw new InvalidOperationException("Conversão de 'Units' não é suportada.");
            }

            throw new NotSupportedException($"Conversão de {from} para {to} não suportada.");
        }
    }
}
