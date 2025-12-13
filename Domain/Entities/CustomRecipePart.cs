namespace PrecificacaoConfeitaria.Domain.Entities {
	public class CustomRecipePart {
		public RecipePart BasePart { get; }
		public decimal QuantityInKg { get; }

		public CustomRecipePart(RecipePart basePart, decimal quantityInKg) {
			BasePart = basePart;
			QuantityInKg = quantityInKg;
		}
	}
}
