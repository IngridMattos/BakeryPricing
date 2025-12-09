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
// Simulação de componentes registrados
var availableComponents = new List<RecipeComponent>
{
    new("Massa de Chocolate", RecipeComponentCategory.Massa, 20m, 1m),
    new("Recheio de Brigadeiro", RecipeComponentCategory.Recheio, 30m, 1m),
    new("Cobertura de Morango", RecipeComponentCategory.Cobertura, 25m, 1m)
};

var recipeBuilder = new RecipeBuilder(availableComponents);

// Montar receita
Console.WriteLine("Digite o nome da nova receita:");
string recipeName = Console.ReadLine() ?? "Receita Sem Nome";

var selectedComponents = new List<(string componentName, decimal weight)>();

while (true)
{
    Console.WriteLine("Selecione um componente disponível:");
    foreach (var component in availableComponents)
    {
        Console.WriteLine($"- {component.Name} ({component.Category}) - R$ {component.PricePerKilogram}/kg");
    }

    Console.WriteLine("Digite o nome do componente:");
    string componentName = Console.ReadLine() ?? "";

    Console.WriteLine("Digite o peso (em kg) para este componente:");
    decimal weight = decimal.Parse(Console.ReadLine()!);

    selectedComponents.Add((componentName, weight));

    Console.WriteLine("Deseja adicionar outro componente? (s/n):");
    if (Console.ReadLine()?.ToLower() != "s") break;
}

var newRecipe = recipeBuilder.BuildRecipe(recipeName, selectedComponents);

Console.WriteLine($"\nReceita \"{newRecipe.Name}\" montada com sucesso!");
Console.WriteLine($"Peso total: {newRecipe.TotalWeight} kg");
Console.WriteLine($"Custo total: {newRecipe.TotalCost:C}");

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
