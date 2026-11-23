using APIRestaurante.Models;
using Microsoft.EntityFrameworkCore;

namespace APIRestaurante.Data;

public class AppDbContext : DbContext
{
    // O AppDbContext é a ligação com o banco, feita com o Entity Framework, que é o ORM
    // As opções (qual banco usar e onde ele está) vêm do Program.cs
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    // Cada DbSet vira uma tabela no banco que usa o nome dos models como nome da tabela
    public DbSet<Restaurante> Restaurantes => Set<Restaurante>();
    public DbSet<Prato> Pratos => Set<Prato>();

    // Aqui explicamos para o EF como as tabelas se relacionam
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Restaurante>()          // "Um Restaurante...
            .HasMany(r => r.Pratos)                 //  ...tem muitos Pratos (a lista)...
            .WithOne(p => p.Restaurante)            //  ...e cada Prato tem um Restaurante...
            .HasForeignKey(p => p.RestauranteId)    //  ...e a ligação é guardada no RestauranteId...
            .OnDelete(DeleteBehavior.Restrict);     //  ...e não pode apagar restaurante que tem prato."

        modelBuilder.Entity<Prato>()
            .Property(p => p.Preco)
            .HasPrecision(10, 2);
    }
}