using System.Web;
using API.GameStore.Contracts;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

const string buscarJogoEndpoint = "buscarJogo";
const string criarJogoEndpoint = "criarJogo";

List<JogosDto> jogos =
[
    new JogosDto(
        Guid.Parse("c792b243-400f-4eea-b466-0cdef117eda4"), 
        "Street Fighter II", 
        "Luta", 
        48.00M, 
        new DateOnly(1992, 7, 15)
    ),
    new JogosDto(
        Guid.Parse("df4a08f3-4daa-4a09-85ec-0c81cb875e79"), 
        "Final Fantasy XIV", 
        "RPG", 
        120.00M, 
        new DateOnly(2010, 9, 30)
    ),
    new JogosDto(
        Guid.Parse("dba0df63-dda5-40d6-a547-74c8c019a676"), 
        "Sun Haven", 
        "Casual", 
        57.00M, 
        new DateOnly(2023, 3, 10)
    ),
];

app.MapGet("/jogos", () => jogos);

app.MapGet("/jogos/{id:guid}", (Guid id) => jogos.FirstOrDefault(j => j.Id == id))
    .WithName(buscarJogoEndpoint);

app.MapGet("/jogos/filtrar", (string? nome, string? genero, decimal? preco, DateOnly? dataDeLancamento) =>
{
    var jogosFiltrados = jogos.AsQueryable();

    if (!string.IsNullOrEmpty(nome))
    {
        var nomeDecodificado = HttpUtility.UrlDecode(nome);
        jogosFiltrados = jogosFiltrados.Where(j => j.Nome.Contains(nomeDecodificado, StringComparison.OrdinalIgnoreCase));
    }

    if (!string.IsNullOrEmpty(genero))
    {
        jogosFiltrados = jogosFiltrados.Where(j => j.Genero.Equals(genero, StringComparison.OrdinalIgnoreCase));
    }

    if (preco.HasValue)
    {
        jogosFiltrados = jogosFiltrados.Where(j => j.Preco == preco.Value);
    }

    if (dataDeLancamento.HasValue)
    {
        jogosFiltrados = jogosFiltrados.Where(j => j.DataDeLancamento == dataDeLancamento.Value);
    }

    return jogosFiltrados.Any() ? Results.Ok(jogosFiltrados) : Results.NotFound();
});

app.MapPost("/jogos", (CriarJogoDto novoGame) =>
{
    var jogo = new JogosDto(Guid.NewGuid(), novoGame.Nome, novoGame.Genero, novoGame.Preco, novoGame.DataDeLancamento);
    jogos.Add(jogo);
    return Results.CreatedAtRoute(buscarJogoEndpoint, new {id = jogo.Id}, jogo);
}).WithName(criarJogoEndpoint);

app.MapPut("/jogos/{id:guid}", (Guid id, AtualizardJogoDto jogoAtualizado) =>
{
    var index = jogos.FindIndex(j => j.Id == id);

    if (index == -1)
    {
        return Results.NotFound();
    }
    jogos[index] = new JogosDto(id, jogoAtualizado.Nome, jogoAtualizado.Genero, jogoAtualizado.Preco, jogoAtualizado.DataDeLancamento);

    return Results.Ok(jogoAtualizado);
});

app.Run();