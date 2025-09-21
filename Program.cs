using System.Globalization;
using System.Text.Json.Serialization;
using PrecificacaoConfeitaria.Domain.Entities;
using PrecificaçãoConfeitaria.Domain.Enums;


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


//teste de criação de ingrediente

Console.WriteLine("Entre com o nome do ingrediente das suas receitas: ");
    var ingredienteNome = Console.ReadLine();

    Console.WriteLine("Entre com o preço do ingrediente: ");
    var ingredientePreco = decimal.Parse(Console.ReadLine(), CultureInfo.InvariantCulture);

    Console.WriteLine("Entre com a unidade de medida do ingrediente (Grams, Kilograms, Milliliters, Liters): ");
    var ingredientMedida = Console.ReadLine();

    var medidaEnum = Enum.GetValues(typeof(UnitOfMeasure))
     .Cast<UnitOfMeasure>()
     .FirstOrDefault(e => e.ToString().Equals(ingredientMedida, StringComparison.OrdinalIgnoreCase));

        if (medidaEnum == default) {
            Console.WriteLine($"Unidade de medida inválida: \"{ingredientMedida}\".");
            return;
        }

var ingrediente = new Ingredient(ingredienteNome, ingredientePreco, medidaEnum);
Console.WriteLine($"Ingrediente criado: Nome={ingrediente.Name}, Preço={ingrediente.PricePerUnit}, Unidade={ingrediente.IngredientUnitOfMeasure}");

//Grams

app.Run();

public record Todo(int Id, string? Title, DateOnly? DueBy = null, bool IsComplete = false);

[JsonSerializable(typeof(Todo[]))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{

}
