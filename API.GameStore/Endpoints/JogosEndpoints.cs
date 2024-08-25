using System.Web;
using API.GameStore.Contracts;

namespace API.GameStore.Endpoints;

public static class JogosEndpoints 
{
    private const string BuscarJogoEndpoint = "buscarJogo";
    private const string CriarJogoEndpoint = "criarJogo";
    private const string AtualizarJogoEndpoint = "atualizarJogo";

    private static readonly List<JogosDto> Jogos =
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

    public static RouteGroupBuilder MapearEndpoints(this WebApplication app)
    {
        var grupoJogos = app.MapGroup("jogos").WithParameterValidation();
        
        // GET /jogos
        grupoJogos.MapGet("/", () => Jogos);

        // GET /jogos/{id}
        grupoJogos.MapGet("/{id:guid}", (Guid id) => Jogos.FirstOrDefault(j => j.Id == id))
            .WithName(BuscarJogoEndpoint);

        // GET /jogos/filtrar
        grupoJogos.MapGet("/filtrar", (string? nome, string? genero, decimal? preco, DateOnly? dataDeLancamento) =>
        {
            var jogosFiltrados = Jogos.AsQueryable();

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

        
        //POST /jogos
        grupoJogos.MapPost("/", (CriarJogoDto novoGame) =>
        {
            var jogo = new JogosDto(Guid.NewGuid(), novoGame.Nome, novoGame.Genero, novoGame.Preco, novoGame.DataDeLancamento);
            Jogos.Add(jogo);
            return Results.CreatedAtRoute(BuscarJogoEndpoint, new {id = jogo.Id}, jogo);
        }).WithName(CriarJogoEndpoint);

        //PUT /jogos/{id}
        grupoJogos.MapPut("/{id:guid}", (Guid id, AtualizardJogoDto jogoAtualizado) =>
        {
            var index = Jogos.FindIndex(j => j.Id == id);

            if (index == -1)
            {
                return Results.NotFound();
            }
            Jogos[index] = new JogosDto(id, jogoAtualizado.Nome, jogoAtualizado.Genero, jogoAtualizado.Preco, jogoAtualizado.DataDeLancamento);

            return Results.Ok(jogoAtualizado);
        }).WithName(AtualizarJogoEndpoint);

        //DELETE /jogos/{id}
        grupoJogos.MapDelete("/{id:guid}", (Guid id) =>
        {
            var jogo = Jogos.FirstOrDefault(j => j.Id == id);
            if (jogo is null)
            {
                return Results.NotFound();
            }

            Jogos.Remove(jogo);
            return Results.NoContent();
        });
        
        //DELETE /jogos/limpeza
        grupoJogos.MapDelete("/limpeza", () =>
        {
            Jogos.Clear();
            return Results.NoContent();
        });

        return grupoJogos;
    }
}