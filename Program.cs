//using System.Globalization;
//using System.Text.Json.Serialization;
//using PrecificacaoConfeitaria.Domain;
//using PrecificaçãoConfeitaria.Domain.Enums;
//using PrecificacaoConfeitaria.Domain.DomainServices;
//using PrecificacaoConfeitaria.Domain.Entities;


//var builder = WebApplication.CreateSlimBuilder(args);

//builder.Services.ConfigureHttpJsonOptions(options =>
//{
//    options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
//});

//var app = builder.Build();

//var sampleTodos = new Todo[] {
//    new(1, "Walk the dog"),
//    new(2, "Do the dishes", DateOnly.FromDateTime(DateTime.Now)),
//    new(3, "Do the laundry", DateOnly.FromDateTime(DateTime.Now.AddDays(1))),
//    new(4, "Clean the bathroom"),
//    new(5, "Clean the car", DateOnly.FromDateTime(DateTime.Now.AddDays(2)))
//};

//var todosApi = app.MapGroup("/todos");
//todosApi.MapGet("/", () => sampleTodos);
//todosApi.MapGet("/{id}", (int id) =>
//    sampleTodos.FirstOrDefault(a => a.Id == id) is { } todo
//        ? Results.Ok(todo)
//        : Results.NotFound());

//Console.WriteLine("Entre com o nome do ingrediente das suas receitas: ");
//var ingredienteNome = Console.ReadLine();

//Console.WriteLine("Entre com o preço do ingrediente por kg: ");
//var ingredientePreco = decimal.Parse(Console.ReadLine(), CultureInfo.InvariantCulture);

//// Crio Primeiro Ingrediente
//var ingrediente = new Ingredient(ingredienteNome, ingredientePreco);
//Console.WriteLine($"Ingrediente criado: Nome={ingrediente.Name}, Preço={ingrediente.PricePerKilogram}");

//Console.WriteLine("Entre com a quantidade do ingrediente na receita:  ");
//var QuantidadeDoIngredienteNaReceita = decimal.Parse(Console.ReadLine(), CultureInfo.InvariantCulture);

//Console.WriteLine("Quanto a receita pede? (Grams, Kilograms, Milliliters, Liters): ");
//var UnidadeDeMedidaNaReceita = Console.ReadLine();

//var medidaIngredienteReceita = Enum.GetValues(typeof(UnitOfMeasure)).Cast<UnitOfMeasure>().FirstOrDefault(e => e.ToString().Equals(UnidadeDeMedidaNaReceita, StringComparison.OrdinalIgnoreCase));

//var receitaItem = new RecipeItem(ingrediente, QuantidadeDoIngredienteNaReceita, medidaIngredienteReceita);
//Console.WriteLine($"Item da receita criado: Ingrediente={receitaItem.Ingredient.Name}, Quantidade={receitaItem.Quantity}, Unidade={receitaItem.Unit}");


////ingrediente 2 :
//Console.WriteLine("Entre com o nome do SEGUNDO ingrediente das suas receitas: ");
//var ingredienteNome2 = Console.ReadLine();

//Console.WriteLine("Entre com o preço do ingrediente2 tbm em kg: ");
//var ingredientePreco2 = decimal.Parse(Console.ReadLine(), CultureInfo.InvariantCulture);


//var ingrediente2 = new Ingredient(ingredienteNome2, ingredientePreco2);
//Console.WriteLine($"Ingrediente criado: Nome={ingrediente2.Name}, Preço={ingrediente2.PricePerKilogram}");

//Console.WriteLine("Entre com a quantidade do ingrediente na receita:  ");
//var QuantidadeDoIngredienteNaReceita2 = decimal.Parse(Console.ReadLine(), CultureInfo.InvariantCulture);

//Console.WriteLine("Entre com quantidade do ingrediente na receita (Grams, Kilograms, Milliliters, Liters): ");
//var UnidadeDeMedidaNaReceita2 = Console.ReadLine();

//var medidaIngredienteReceita2 = Enum.GetValues(typeof(UnitOfMeasure))
//    .Cast<UnitOfMeasure>()
//    .FirstOrDefault(e => e.ToString().Equals(UnidadeDeMedidaNaReceita2, StringComparison.OrdinalIgnoreCase));

//var receitaItem2 = new RecipeItem(ingrediente2, QuantidadeDoIngredienteNaReceita2, medidaIngredienteReceita2);
//Console.WriteLine($"Item da receita criado: Ingrediente={receitaItem2.Ingredient.Name}, Quantidade={receitaItem2.Quantity}, Unidade={receitaItem2 .Unit}");
//// criação da receita
//Console.WriteLine("Entre com o nome da receita: ");
//var nomeDaReceita = Console.ReadLine();

//List<RecipeItem> listaDeItensDaReceita = new List<RecipeItem>();
//listaDeItensDaReceita.Add(receitaItem);
//listaDeItensDaReceita.Add(receitaItem2);

//var receita = new Recipe(nomeDaReceita, listaDeItensDaReceita);
//Console.WriteLine($"Receita criada: Nome={receita.Name}, Itens={receita.IngredientsAndQuantity.Count}");
//// cálculo do custo da receita
//var recipeCosts = new RecipeCosts();
//var custoTotalDaReceita = recipeCosts.CalculateTotalRecipeCost(receita);
//Console.WriteLine($"Custo total da receita \"{receita.Name}\": {custoTotalDaReceita.ToString("C", CultureInfo.CurrentCulture)}");

//app.Run();

//public record Todo(int Id, string? Title, DateOnly? DueBy = null, bool IsComplete = false);

//[JsonSerializable(typeof(Todo[]))]
//internal partial class AppJsonSerializerContext : JsonSerializerContext
//{

//}

using System.Globalization;
using System.Text.Json.Serialization;
using PrecificacaoConfeitaria.Domain.Entities;
using PrecificacaoConfeitaria.Domain.DomainServices;
using PrecificaçãoConfeitaria.Domain.Enums;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
});

var app = builder.Build();

// --- Entrada de ingredientes para a receita ---
List<RecipeItem> recipeItems = new List<RecipeItem>();
bool adicionarMaisIngredientes = true;

while (adicionarMaisIngredientes)
{
    // Nome do ingrediente
    string ingredientName;
    do
    {
        Console.WriteLine("Digite o nome do ingrediente: ");
        ingredientName = (Console.ReadLine() ?? "").Trim();
    } while (string.IsNullOrEmpty(ingredientName));

    // Verifica se é vendido por unidade
    Console.WriteLine("O ingrediente é vendido por unidade? (s/n): ");
    bool isByUnit = (Console.ReadLine() ?? "").Trim().ToLower() == "s";

    decimal pricePerKg = 0;
    decimal pricePerUnit = 0;
    int unitsPerPackage = 1;
    UnitOfMeasure ingredientUnit = isByUnit ? UnitOfMeasure.Units : UnitOfMeasure.Kilograms;

    if (isByUnit)
    {
        while (true)
        {
            Console.WriteLine("Digite o preço da embalagem: ");
            if (decimal.TryParse(Console.ReadLine()?.Replace(',', '.') ?? "0", NumberStyles.Any, CultureInfo.InvariantCulture, out pricePerUnit) && pricePerUnit > 0)
                break;
            Console.WriteLine("Entrada inválida. Digite um número maior que zero.");
        }

        while (true)
        {
            Console.WriteLine("Quantas unidades há na embalagem? ");
            if (int.TryParse(Console.ReadLine(), out unitsPerPackage) && unitsPerPackage > 0)
                break;
            Console.WriteLine("Entrada inválida. Digite um número inteiro maior que zero.");
        }
    }
    else
    {
        while (true)
        {
            Console.WriteLine("Digite o preço por kg: ");
            if (decimal.TryParse(Console.ReadLine()?.Replace(',', '.') ?? "0", NumberStyles.Any, CultureInfo.InvariantCulture, out pricePerKg) && pricePerKg >= 0)
                break;
            Console.WriteLine("Entrada inválida. Digite um número válido.");
        }
    }

    var ingredient = new Ingredient(ingredientName, pricePerKg, ingredientUnit, pricePerUnit, unitsPerPackage);
    Console.WriteLine($"Ingrediente criado: Nome={ingredient.Name}, Preço/kg={ingredient.PricePerKilogram}, Preço/embalagem={ingredient.PricePerUnit}, Unidades/embalagem={ingredient.UnitsPerPackage}, Unidade={ingredient.Unit}");

    // Quantidade na receita
    decimal quantity;
    while (true)
    {
        Console.WriteLine("Digite a quantidade do ingrediente na receita: ");
        if (decimal.TryParse(Console.ReadLine()?.Replace(',', '.') ?? "0", NumberStyles.Any, CultureInfo.InvariantCulture, out quantity) && quantity > 0)
            break;
        Console.WriteLine("Entrada inválida. Digite um número maior que zero.");
    }

    // Unidade da receita
    UnitOfMeasure recipeUnit;
    if (ingredient.Unit == UnitOfMeasure.Units)
    {
        recipeUnit = UnitOfMeasure.Units;
        Console.WriteLine("Ingrediente vendido por unidade, unidade da receita definida automaticamente como 'Units'.");
    }
    else
    {
        while (true)
        {
            Console.WriteLine("Digite a unidade de medida (Grams, Kilograms, Milliliters, Liters): ");
            string unitStr = Console.ReadLine() ?? "";
            if (Enum.TryParse<UnitOfMeasure>(unitStr, true, out recipeUnit) && recipeUnit != UnitOfMeasure.Units)
                break;
            Console.WriteLine("Entrada inválida. Digite uma unidade válida.");
        }
    }

    var recipeItem = new RecipeItem(ingredient, quantity, recipeUnit);
    recipeItems.Add(recipeItem);

    Console.WriteLine($"Ingrediente adicionado à receita: Nome={recipeItem.Ingredient.Name}, Quantidade={recipeItem.Quantity}, Unidade={recipeItem.Unit}");

    Console.WriteLine("Deseja adicionar outro ingrediente? (s/n): ");
    adicionarMaisIngredientes = (Console.ReadLine() ?? "").Trim().ToLower() == "s";
}

// --- Criação da receita ---
string recipeName;
do
{
    Console.WriteLine("Digite o nome da receita: ");
    recipeName = (Console.ReadLine() ?? "").Trim();
} while (string.IsNullOrEmpty(recipeName));

var recipe = new Recipe(recipeName, recipeItems);
Console.WriteLine($"Receita criada: Nome={recipe.Name}, Itens={recipe.IngredientsAndQuantity.Count}");

// --- Cálculo do custo total da receita ---
decimal totalCost = 0m;
foreach (var item in recipe.IngredientsAndQuantity)
{
    if (item.Unit == UnitOfMeasure.Units)
    {
        decimal pricePerSingleUnit = item.Ingredient.PricePerUnit / item.Ingredient.UnitsPerPackage;
        totalCost += pricePerSingleUnit * item.Quantity;
    }
    else
    {
        totalCost += UnitConverter.Convert(item.Quantity, item.Unit, UnitOfMeasure.Kilograms) * item.Ingredient.PricePerKilogram;
    }
}

Console.WriteLine($"Custo total da receita \"{recipe.Name}\": {totalCost.ToString("C", CultureInfo.CurrentCulture)}");

// --- API de exemplo (mantida) ---
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
