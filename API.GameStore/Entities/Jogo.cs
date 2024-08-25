namespace API.GameStore.Entities;

public class Jogo
{
    public Guid Id { get; set; }
    public required string Nome { get; set; }
    public int IdGenero { get; set; }
    public Genero? Genero { get; set; }
    public decimal Preco { get; set; }
    public DateOnly DataDeLancamento { get; set; }
}