using System.Globalization;
using PrecificacaoConfeitaria.Domain.Entities;
using PrecificacaoConfeitaria.Domain.Enums;
using PrecificacaoConfeitaria.Domain.Services;

var builder = WebApplication.CreateSlimBuilder(args);
var app = builder.Build();

// -------------------------------------------//
// Inicialização dos serviços da camada de domínio

var stockService = new StockService();
var indirectCosts = new List<IndirectCost>();
var recipes = new List<Recipe>();

// -------------------------------------------//
// Menu principal

while (true)
{
    Console.WriteLine("\n=== Sistema de Precificação Confeitaria ===");
    Console.WriteLine("1. Registrar Ingrediente e Estoque");
    Console.WriteLine("2. Registrar Custo Indireto Mensal");
    Console.WriteLine("3. Registrar Receita");
    Console.WriteLine("4. Calcular Custo de uma Receita");
    Console.WriteLine("5. Listar Receitas e Custos");
    Console.WriteLine("6. Sair");
    Console.Write("Escolha uma opção: ");
    string? option = Console.ReadLine();

    switch (option)
    {
        case "1":
            RegisterIngredientAndStock(stockService);
            break;
        case "2":
            RegisterIndirectCost(indirectCosts);
            break;
        case "3":
            RegisterRecipe(recipes, stockService);
            break;
        case "4":
            PriceRecipe(recipes, stockService, indirectCosts);
            break;
        case "5":
            ListAllRecipes(recipes);
            break;
        case "6":
            Console.WriteLine("Saindo do sistema...");
            return;
        default:
            Console.WriteLine("Opção inválida. Tente novamente.");
            break;
    }
}

// -------------------------------------------//
// Funções auxiliares

void RegisterIngredientAndStock(StockService stockService)
{
    Console.WriteLine("\n=== Registrar Ingrediente ===");

    Console.Write("Nome do ingrediente: ");
    string name = Console.ReadLine() ?? "";

    Console.Write("É vendido por unidade? (s/n): ");
    string isUnit = Console.ReadLine()?.ToLower() ?? "n";

    UnitOfMeasure unit = isUnit == "s" ? UnitOfMeasure.Units : UnitOfMeasure.Kilograms;

    Console.Write("Preço do pacote (se for unidade, preço do pacote): ");
    decimal price = decimal.Parse(Console.ReadLine()!, CultureInfo.InvariantCulture);

    Console.Write(isUnit == "s"
        ? "Quantas unidades existem no pacote? "
        : "Peso do pacote em kg: ");

    decimal amount = decimal.Parse(Console.ReadLine()!, CultureInfo.InvariantCulture);

    var ingredient = new Ingredient(name,
        unit == UnitOfMeasure.Kilograms ? price : 0,
        unit,
        unit == UnitOfMeasure.Units ? price : 0,
        unit == UnitOfMeasure.Units ? (int)amount : 1);

    var stockItem = new StockItem(
        ingredient,
        unit == UnitOfMeasure.Kilograms ? amount : 0,
        price);

    stockService.AddStockItem(stockItem);

    Console.WriteLine($"Ingrediente {name} registrado e adicionado ao estoque!");
}

void RegisterIndirectCost(List<IndirectCost> indirectCosts)
{
    Console.WriteLine("\n=== Registrar Custo Indireto Mensal ===");

    Console.Write("Descrição (ex: aluguel, luz): ");
    string desc = Console.ReadLine() ?? "";

    Console.Write("Valor mensal: ");
    decimal value = decimal.Parse(Console.ReadLine()!, CultureInfo.InvariantCulture);

    indirectCosts.Add(new IndirectCost(desc, value));

    Console.WriteLine("Custo indireto registrado com sucesso!");
}

void RegisterRecipe(List<Recipe> recipes, StockService stock)
{
    Console.WriteLine("\n=== Registrar Receita ===");

    Console.Write("Nome da receita: ");
    string recipeName = Console.ReadLine() ?? "";

    var recipe = new Recipe(recipeName);

    while (true)
    {
        Console.WriteLine("\nIngredientes disponíveis no estoque:");
        foreach (var item in stock.GetAllStockItems())
            Console.WriteLine($"- {item.Ingredient.Name}");

        Console.Write("Escolha um ingrediente: ");
        string ingredientName = Console.ReadLine() ?? "";

        var ingredient = stock.GetIngredientByName(ingredientName);
        if (ingredient == null)
        {
            Console.WriteLine("Ingrediente não encontrado!");
            continue;
        }

        Console.Write("Quantidade usada (com base na unidade do ingrediente): ");
        decimal quantity = decimal.Parse(Console.ReadLine()!, CultureInfo.InvariantCulture);

        recipe.AddComponent(new RecipeComponent(ingredient, quantity));

        Console.Write("Adicionar outro ingrediente? (s/n): ");
        if (Console.ReadLine()?.ToLower() != "s")
            break;
    }

    recipes.Add(recipe);
    Console.WriteLine($"Receita {recipeName} registrada com sucesso!");
}

void PriceRecipe(List<Recipe> recipes, StockService stock, List<IndirectCost> indirectCosts)
{
    Console.WriteLine("\n=== Calcular Custo da Receita ===");

    Console.Write("Nome da receita: ");
    string name = Console.ReadLine() ?? "";

    var recipe = recipes.FirstOrDefault(r => r.Name == name);

    if (recipe == null)
    {
        Console.WriteLine("Receita não encontrada!");
        return;
    }

    Console.Write("Qual a produção total do mês (em kg)? ");
    decimal totalKgMonth = decimal.Parse(Console.ReadLine()!, CultureInfo.InvariantCulture);

    var allocator = new IndirectCostAllocator(indirectCosts, totalKgMonth);
    var recipeCosts = new RecipeCosts(stock);

    decimal ingredientCost = recipeCosts.CalculateTotalRecipeCost(recipe);
    decimal indirectCost = allocator.AllocateToRecipe(recipe.GetTotalWeightInKg());
    decimal total = ingredientCost + indirectCost;

    Console.WriteLine("\n=== Resultado da Precificação ===");
    Console.WriteLine($"Receita: {recipe.Name}");
    Console.WriteLine($"Peso total: {recipe.GetTotalWeightInKg()} kg");
    Console.WriteLine($"Custo dos ingredientes: {ingredientCost:C}");
    Console.WriteLine($"Custos indiretos alocados: {indirectCost:C}");
    Console.WriteLine($"Custo total: {total:C}");
}

void ListAllRecipes(List<Recipe> recipes)
{
    Console.WriteLine("\n=== Lista de Receitas Cadastradas ===");

    foreach (var r in recipes)
    {
        Console.WriteLine($"- {r.Name} ({r.GetTotalWeightInKg()} kg)");
    }
}

// --------------------------------------------//

app.Run();
