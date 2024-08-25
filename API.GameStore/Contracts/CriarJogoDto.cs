namespace API.GameStore.Contracts;

public record CriarJogoDto(
    string Nome,
    string Genero,
    decimal Preco,
    DateOnly DataDeLancamento
);  