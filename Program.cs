using System.Globalization;
using System.Text.Json.Serialization;
using PrecificacaoConfeitaria.Domain;
using PrecificaçãoConfeitaria.Domain.Enums;
using PrecificacaoConfeitaria.Domain.DomainServices;
using PrecificacaoConfeitaria.Domain.Entities;


var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
});

var app = builder.Build();

var sampleTodos = new Todo[] {
    new(1, "Walk the dog"),
    new(2, "Do the dishes", DateOnly.FromDateTime(DateTime.Now)),
    new(3, "Do the laundry", DateOnly.FromDateTime(DateTime.Now.AddDays(1))),
    new(4, "Clean the bathroom"),
    new(5, "Clean the car", DateOnly.FromDateTime(DateTime.Now.AddDays(2)))
};

var todosApi = app.MapGroup("/todos");
todosApi.MapGet("/", () => sampleTodos);
todosApi.MapGet("/{id}", (int id) =>
    sampleTodos.FirstOrDefault(a => a.Id == id) is { } todo
        ? Results.Ok(todo)
        : Results.NotFound());

Console.WriteLine("Entre com o nome do ingrediente das suas receitas: ");
var ingredienteNome = Console.ReadLine();

Console.WriteLine("Entre com o preço do ingrediente por kg: ");
var ingredientePreco = decimal.Parse(Console.ReadLine(), CultureInfo.InvariantCulture);

// Crio Primeiro Ingrediente
var ingrediente = new Ingredient(ingredienteNome, ingredientePreco);
Console.WriteLine($"Ingrediente criado: Nome={ingrediente.Name}, Preço={ingrediente.PricePerUnit}");

Console.WriteLine("Entre com a quantidade do ingrediente na receita:  ");
var QuantidadeDoIngredienteNaReceita = decimal.Parse(Console.ReadLine(), CultureInfo.InvariantCulture);

Console.WriteLine("Quanto a receita pede? (Grams, Kilograms, Milliliters, Liters): ");
var UnidadeDeMedidaNaReceita = Console.ReadLine();

var medidaIngredienteReceita = Enum.GetValues(typeof(UnitOfMeasure)).Cast<UnitOfMeasure>().FirstOrDefault(e => e.ToString().Equals(UnidadeDeMedidaNaReceita, StringComparison.OrdinalIgnoreCase));

var receitaItem = new RecipeItem(ingrediente, QuantidadeDoIngredienteNaReceita, medidaIngredienteReceita);
Console.WriteLine($"Item da receita criado: Ingrediente={receitaItem.Ingredient.Name}, Quantidade={receitaItem.Quantity}, Unidade={receitaItem.Unit}");


//ingrediente 2 :
Console.WriteLine("Entre com o nome do SEGUNDO ingrediente das suas receitas: ");
var ingredienteNome2 = Console.ReadLine();

Console.WriteLine("Entre com o preço do ingrediente2 tbm em kg: ");
var ingredientePreco2 = decimal.Parse(Console.ReadLine(), CultureInfo.InvariantCulture);


var ingrediente2 = new Ingredient(ingredienteNome2, ingredientePreco2);
Console.WriteLine($"Ingrediente criado: Nome={ingrediente2.Name}, Preço={ingrediente2.PricePerUnit}");

Console.WriteLine("Entre com a quantidade do ingrediente na receita:  ");
var QuantidadeDoIngredienteNaReceita2 = decimal.Parse(Console.ReadLine(), CultureInfo.InvariantCulture);

Console.WriteLine("Entre com quantidade do ingrediente na receita (Grams, Kilograms, Milliliters, Liters): ");
var UnidadeDeMedidaNaReceita2 = Console.ReadLine();

var medidaIngredienteReceita2 = Enum.GetValues(typeof(UnitOfMeasure))
    .Cast<UnitOfMeasure>()
    .FirstOrDefault(e => e.ToString().Equals(UnidadeDeMedidaNaReceita2, StringComparison.OrdinalIgnoreCase));

var receitaItem2 = new RecipeItem(ingrediente2, QuantidadeDoIngredienteNaReceita2, medidaIngredienteReceita2);
Console.WriteLine($"Item da receita criado: Ingrediente={receitaItem2.Ingredient.Name}, Quantidade={receitaItem2.Quantity}, Unidade={receitaItem2 .Unit}");
// criação da receita
Console.WriteLine("Entre com o nome da receita: ");
var nomeDaReceita = Console.ReadLine();

List<RecipeItem> listaDeItensDaReceita = new List<RecipeItem>();
listaDeItensDaReceita.Add(receitaItem);
listaDeItensDaReceita.Add(receitaItem2);

var receita = new Recipe(nomeDaReceita, listaDeItensDaReceita);
Console.WriteLine($"Receita criada: Nome={receita.Name}, Itens={receita.IngredientsAndQuantity.Count}");
// cálculo do custo da receita
var recipeCosts = new RecipeCosts();
var custoTotalDaReceita = recipeCosts.CalculateTotalRecipeCost(receita);
Console.WriteLine($"Custo total da receita \"{receita.Name}\": {custoTotalDaReceita.ToString("C", CultureInfo.CurrentCulture)}");

app.Run();

public record Todo(int Id, string? Title, DateOnly? DueBy = null, bool IsComplete = false);

[JsonSerializable(typeof(Todo[]))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{

}
