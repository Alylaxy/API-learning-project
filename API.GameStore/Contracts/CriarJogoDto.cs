namespace API.GameStore.Contracts;

public record class CriarJogoDto(
    string Nome,
    string Genero,
    decimal Preco,
    DateOnly DataDeLancamento
);  