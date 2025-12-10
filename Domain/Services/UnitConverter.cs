using PrecificacaoConfeitaria.Domain.Enums;

namespace PrecificacaoConfeitaria.Domain.Services {
    public static class UnitConverter {
        private static readonly Dictionary<(UnitOfMeasure, UnitOfMeasure), Func<decimal, decimal, decimal, decimal>> Conversions =
            new()
            {
                { (UnitOfMeasure.Grams, UnitOfMeasure.Kilograms), (quantity, _, _) => quantity / 1000m },
                { (UnitOfMeasure.Kilograms, UnitOfMeasure.Grams), (quantity, _, _) => quantity * 1000m },
                { (UnitOfMeasure.Milliliters, UnitOfMeasure.Liters), (quantity, _, _) => quantity / 1000m },
                { (UnitOfMeasure.Liters, UnitOfMeasure.Milliliters), (quantity, _, _) => quantity * 1000m },
                { (UnitOfMeasure.Grams, UnitOfMeasure.Milliliters), (quantity, _, density) => quantity / density },
                { (UnitOfMeasure.Milliliters, UnitOfMeasure.Grams), (quantity, _, density) => quantity * density },
                // Adicionar outras conversões aqui
            };

        public static decimal Convert(decimal quantity, UnitOfMeasure from, UnitOfMeasure to, decimal pricePerUnit = 0, decimal density = 1) {
            if (quantity <= 0)
                throw new ArgumentException("A quantidade deve ser maior que zero.");

            if (density <= 0)
                throw new ArgumentException("A densidade deve ser maior que zero.");

            if (from == to) return quantity;

            if (Conversions.TryGetValue((from, to), out var conversion))
                return conversion(quantity, pricePerUnit, density);

            if (from == UnitOfMeasure.Units) {
                if (pricePerUnit <= 0)
                    throw new InvalidOperationException("Peso por unidade deve ser informado para conversão de 'Units'.");

                if (to == UnitOfMeasure.Kilograms) return quantity * pricePerUnit;
                if (to == UnitOfMeasure.Grams) return (quantity * pricePerUnit) * 1000m;
                if (to == UnitOfMeasure.Milliliters) return (quantity * pricePerUnit) * 1000m / density;
                if (to == UnitOfMeasure.Liters) return (quantity * pricePerUnit) / density;
            }

            throw new NotSupportedException($"A conversão de {from} para {to} não é suportada. Verifique as unidades e tente novamente.");
        }
    }
}
