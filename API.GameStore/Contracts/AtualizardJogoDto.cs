namespace API.GameStore.Contracts;

public record AtualizardJogoDto(
    string Nome,
    string Genero,
    decimal Preco,
    DateOnly DataDeLancamento
);