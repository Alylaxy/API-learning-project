namespace API.GameStore.Contracts;

public record JogosDto(
    Guid Id, 
    string Nome, 
    string Genero, 
    decimal Preco, 
    DateOnly DataDeLancamento
);