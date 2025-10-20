using System.Globalization;
using System.Text.Json.Serialization;
using PrecificacaoConfeitaria.Domain.Entities;
using PrecificacaoConfeitaria.Domain.Enums;
using PrecificacaoConfeitaria.Domain.Services;


var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
});

var app = builder.Build();

// === Entrada de dados do usuário para criar a receita === \o/
List<RecipeItem> recipeItems = new List<RecipeItem>();

while (true)
{
    Console.WriteLine("Digite o nome do ingrediente:");
    string ingredientName = Console.ReadLine() ?? "";

    Console.WriteLine("O ingrediente é vendido por unidade? (s/n):");
    string soldByUnit = Console.ReadLine()?.ToLower() ?? "n";

    Ingredient ingredient;

    if (soldByUnit == "s")
    {
        Console.WriteLine("Digite o preço por pacote/unidade do ingrediente:");
        decimal pricePerUnit = decimal.Parse(Console.ReadLine()!, CultureInfo.InvariantCulture);

        Console.WriteLine("Quantas unidades vem no pacote?");
        int unitsPerPackage = int.Parse(Console.ReadLine()!);

        ingredient = new Ingredient(ingredientName, 0, UnitOfMeasure.Units, pricePerUnit)
        {
            UnitsPerPackage = unitsPerPackage
        };
    }
    else
    {
        Console.WriteLine("Digite o preço por kg:");
        decimal pricePerKg = decimal.Parse(Console.ReadLine()!, CultureInfo.InvariantCulture);

        ingredient = new Ingredient(ingredientName, pricePerKg);
    }

    Console.WriteLine("Digite a quantidade do ingrediente na receita:");
    decimal quantity = decimal.Parse(Console.ReadLine()!, CultureInfo.InvariantCulture);

    UnitOfMeasure unit;
    if (ingredient.Unit == UnitOfMeasure.Units)
    {
        unit = UnitOfMeasure.Units;
        Console.WriteLine("Ingrediente vendido por unidade, unidade da receita definida automaticamente como 'Units'.");
    }
    else
    {
        Console.WriteLine("Digite a unidade de medida (Grams, Kilograms, Milliliters, Liters):");
        string unitInput = Console.ReadLine() ?? "Grams";

        unit = Enum.TryParse(unitInput, true, out UnitOfMeasure parsedUnit) ? parsedUnit : UnitOfMeasure.Grams;
    }

    var recipeItem = new RecipeItem(ingredient, quantity, unit);
    recipeItems.Add(recipeItem);

    Console.WriteLine($"Ingrediente adicionado à receita: Nome={ingredient.Name}, Quantidade={quantity}, Unidade={unit}");

    Console.WriteLine("Deseja adicionar outro ingrediente? (s/n):");
    string addAnother = Console.ReadLine()?.ToLower() ?? "n";
    if (addAnother != "s") break;
}

// Criação da receita
Console.WriteLine("Digite o nome da receita:");
string recipeName = Console.ReadLine() ?? "Receita";
var recipe = new Recipe(recipeName, recipeItems);

// === Cálculo do custo da receita ===
var recipeCostsService = new RecipeCosts();
decimal totalCost = recipeCostsService.CalculateTotalRecipeCost(recipe);
Console.WriteLine($"\nCusto total da receita \"{recipe.Name}\": {totalCost:C}");

// === Cálculo do preço final com margem de lucro ===
Console.WriteLine("Digite a margem de lucro desejada (%) sobre o custo da receita:");
decimal profitPercentage = decimal.Parse(Console.ReadLine()!, CultureInfo.InvariantCulture);

var pricingService = new RecipePricingService();
decimal finalPrice = pricingService.CalculateFinalPrice(recipe, profitPercentage);
Console.WriteLine($"Preço final da receita com {profitPercentage}% de lucro: {finalPrice:C}");

// --------------------------------------------//

var sampleTodos = new Todo[]
{
    new(1, "Passear com o cachorro"),
    new(2, "Lavar a louça", DateOnly.FromDateTime(DateTime.Now)),
    new(3, "Lavar roupa", DateOnly.FromDateTime(DateTime.Now.AddDays(1))),
    new(4, "Limpar o banheiro"),
    new(5, "Limpar o carro", DateOnly.FromDateTime(DateTime.Now.AddDays(2)))
};

var todosApi = app.MapGroup("/todos");
todosApi.MapGet("/", () => sampleTodos);
todosApi.MapGet("/{id}", (int id) =>
    sampleTodos.FirstOrDefault(a => a.Id == id) is { } todo
        ? Results.Ok(todo)
        : Results.NotFound());

app.Run();

public record Todo(int Id, string? Title, DateOnly? DueBy = null, bool IsComplete = false);

[JsonSerializable(typeof(Todo[]))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{
}
