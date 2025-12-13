using System.Globalization;
using PrecificacaoConfeitaria.Domain.Entities;
using PrecificacaoConfeitaria.Domain.Enums;
using PrecificacaoConfeitaria.Domain.Services;

var builder = WebApplication.CreateSlimBuilder(args);
var app = builder.Build();

// -------------------------------------------//
// Inicialização dos serviços da camada de domínio

Console.WriteLine("=== TESTE SISTEMA DE PRECIFICAÇÃO CONFEITARIA ===");

// --------------------------------------------------
// 1️⃣ ESTOQUE (tudo em KG internamente)

// Farinha: 5kg por R$ 25
var farinha = new Ingredient("Farinha", IngredientMeasureType.Weight);
var stockService = new StockService();
stockService.AddStockItem(new StockItem(
    farinha,
    quantityInKg: 5m,
    totalPrice: 25m
));

// Ovo: 30 unidades, 50g cada = 1.5kg por R$ 24
var ovo = new Ingredient("Ovo", IngredientMeasureType.Weight);
stockService.AddStockItem(new StockItem(
    ovo,
    quantityInKg: 1.5m,
    totalPrice: 24m
));

Console.WriteLine("✔ Estoque cadastrado");

// --------------------------------------------------
// 2️⃣ PARTES DE RECEITA (massa, recheio, cobertura...)

var massaType = new RecipePartType("Massa");
var recheioType = new RecipePartType("Recheio");

// Massa de chocolate (para teste vamos usar farinha + ovo)
var massaChocolate = new RecipePart("Massa de Chocolate", massaType);
massaChocolate.AddIngredient(new RecipePartIngredient(farinha, 0.3m)); // 300g
massaChocolate.AddIngredient(new RecipePartIngredient(ovo, 0.2m));     // 200g

// Recheio simples
var recheio = new RecipePart("Recheio Simples", recheioType);
recheio.AddIngredient(new RecipePartIngredient(ovo, 0.1m)); // 100g

Console.WriteLine("✔ Partes de receita criadas");

// --------------------------------------------------
// 3️⃣ RECEITA PERSONALIZADA (cliente escolhe as partes)

var customRecipe = new CustomRecipe("Cliente Maria");
customRecipe.AddPart(massaChocolate);
customRecipe.AddPart(recheio);

Console.WriteLine("✔ Receita personalizada montada");

// --------------------------------------------------
// 4️⃣ CUSTOS INDIRETOS MENSAIS

var indirectCosts = new List<IndirectCost>
{
    new("Aluguel", 1200m),
    new("Luz", 300m),
    new("Água", 150m)
};

// Produção mensal estimada: 100kg
var allocator = new IndirectCostAllocator(indirectCosts, monthlyKg: 100m);

// --------------------------------------------------
// 5️⃣ CÁLCULO FINAL

var partCostService = new RecipePartCostService(stockService);
var pricingService = new RecipePricingService(partCostService, allocator);

// Margem de lucro: 30%
decimal finalPrice = pricingService.Calculate(customRecipe, profitPercent: 30);

Console.WriteLine("\n=== RESULTADO ===");
Console.WriteLine($"Cliente: {customRecipe.ClientName}");
Console.WriteLine($"Preço final sugerido: R$ {finalPrice:F2}");

Console.WriteLine("\n=== FIM DO TESTE ===");


// --------------------------------------------//

app.Run();
