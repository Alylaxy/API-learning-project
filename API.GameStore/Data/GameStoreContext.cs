using API.GameStore.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.GameStore.Data;

public class GameStoreContext(DbContextOptions<GameStoreContext> options) : DbContext(options)
{
    public DbSet<Jogo> Jogos => Set<Jogo>();
    public DbSet<Genero> Generos => Set<Genero>();
}