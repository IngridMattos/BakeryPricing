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

// -------------------------------------------//
// Inicialização de serviços e dados
var categories = new List<Category>
{
    new("Massa"),
    new("Recheio"),
    new("Cobertura"),
    new("Receitas Completas")
};

var stockService = new StockService();
var recipeManagementService = new RecipeManagementService();

// Menu principal
while (true)
{
    Console.WriteLine("\n=== Sistema de Precificação Confeitaria ===");
    Console.WriteLine("1. Registrar Ingrediente");
    Console.WriteLine("2. Registrar Receita");
    Console.WriteLine("3. Precificar Receita");
    Console.WriteLine("4. Listar Todas as Receitas");
    Console.WriteLine("5. Sair");
    Console.Write("Escolha uma opção: ");
    string? option = Console.ReadLine();

    switch (option)
    {
        case "1":
            RegisterIngredient(stockService);
            break;
        case "2":
            RegisterRecipe(recipeManagementService, categories);
            break;
        case "3":
            PriceRecipe(recipeManagementService, stockService);
            break;
        case "4":
            ListAllRecipes(recipeManagementService);
            break;
        case "5":
            Console.WriteLine("Saindo do sistema...");
            return;
        default:
            Console.WriteLine("Opção inválida. Tente novamente.");
            break;
    }
}

// -------------------------------------------//
// Funções auxiliares

void RegisterIngredient(StockService stockService)
{
    Console.WriteLine("\n=== Registrar Ingrediente ===");
    Console.Write("Digite o nome do ingrediente: ");
    string ingredientName = Console.ReadLine() ?? "";

    Console.Write("Digite o preço (por kg ou por unidade): ");
    decimal price = decimal.Parse(Console.ReadLine()!, CultureInfo.InvariantCulture);

    Console.Write("O ingrediente é vendido por unidade? (s/n): ");
    string soldByUnit = Console.ReadLine()?.ToLower() ?? "n";

    UnitOfMeasure unit = soldByUnit == "s" ? UnitOfMeasure.Units : UnitOfMeasure.Kilograms;

    Console.Write($"Digite o peso comprado (em {unit}): ");
    decimal weight = decimal.Parse(Console.ReadLine()!, CultureInfo.InvariantCulture);

    var ingredient = new Ingredient(ingredientName, unit == UnitOfMeasure.Kilograms ? price : 0, unit, unit == UnitOfMeasure.Units ? price : 0);
    var stockItem = new StockItem(ingredient, unit == UnitOfMeasure.Kilograms ? weight : 0, price * weight);
    stockService.AddStockItem(stockItem);

    Console.WriteLine($"Ingrediente {ingredientName} registrado com sucesso!");
}

void RegisterRecipe(RecipeManagementService recipeManagementService, List<Category> categories)
{
    Console.WriteLine("\n=== Registrar Receita ===");
    Console.Write("Digite o nome da receita: ");
    string recipeName = Console.ReadLine() ?? "";

    Console.WriteLine("Selecione uma categoria:");
    for (int i = 0; i < categories.Count; i++)
    {
        Console.WriteLine($"{i + 1}. {categories[i].Name}");
    }
    int categoryIndex = int.Parse(Console.ReadLine()!) - 1;
    var category = categories[categoryIndex];

    Console.Write("Digite o peso da receita (em kg): ");
    decimal recipeWeight = decimal.Parse(Console.ReadLine()!, CultureInfo.InvariantCulture);

    Console.Write("Digite o preço por kg da receita: ");
    decimal recipePricePerKg = decimal.Parse(Console.ReadLine()!, CultureInfo.InvariantCulture);

    var recipeComponent = new RecipeComponent(recipeName, RecipeComponentCategory.Massa, recipePricePerKg, recipeWeight);
    recipeManagementService.AddRecipeComponent(recipeComponent);

    Console.WriteLine($"Receita {recipeName} registrada com sucesso na categoria {category.Name}!");
}

void PriceRecipe(RecipeManagementService recipeManagementService, StockService stockService)
{
    Console.WriteLine("\n=== Precificar Receita ===");
    Console.Write("Digite o nome da nova receita: ");
    string newRecipeName = Console.ReadLine() ?? "";

    var selectedComponents = new List<(string componentName, decimal weight)>();

    while (true)
    {
        Console.WriteLine("Selecione um componente disponível:");
        foreach (var component in recipeManagementService.GetAllComponents())
        {
            Console.WriteLine($"- {component.Name} ({component.Category.Name}) - R$ {component.PricePerKilogram}/kg");
        }

        Console.Write("Digite o nome do componente: ");
        string componentName = Console.ReadLine() ?? "";

        Console.Write("Digite o peso (em kg) para este componente: ");
        decimal weight = decimal.Parse(Console.ReadLine()!, CultureInfo.InvariantCulture);

        selectedComponents.Add((componentName, weight));

        Console.Write("Deseja adicionar outro componente? (s/n): ");
        if (Console.ReadLine()?.ToLower() != "s") break;
    }

    var recipeBuilder = new RecipeBuilder(recipeManagementService.GetAllComponents());
    var newRecipe = recipeBuilder.BuildRecipe(newRecipeName, selectedComponents);

    var recipeCosts = new RecipeCosts(stockService);
    decimal totalCost = recipeCosts.CalculateTotalRecipeCost(newRecipe);

    Console.WriteLine($"\nReceita \"{newRecipe.Name}\" montada com sucesso!");
    Console.WriteLine($"Peso total: {newRecipe.TotalWeight} kg");
    Console.WriteLine($"Custo total: {totalCost:C}");
}

void ListAllRecipes(RecipeManagementService recipeManagementService)
{
    Console.WriteLine("\n=== Listar Todas as Receitas ===");
    recipeManagementService.ListAllRecipes();
}

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
