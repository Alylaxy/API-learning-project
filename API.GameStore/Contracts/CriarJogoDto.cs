using System.ComponentModel.DataAnnotations;

namespace API.GameStore.Contracts;

public record CriarJogoDto(
    [Required][StringLength(50)]string Nome,
    [Required][StringLength(15)]string Genero,
    [Required][Range(0, 600)]decimal Preco,
    DateOnly DataDeLancamento
);  