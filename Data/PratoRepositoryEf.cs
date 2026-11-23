using APIRestaurante.Models;
using APIRestaurante.Services;
using Microsoft.EntityFrameworkCore;

namespace APIRestaurante.Data;

//O repositório usa o AppDbContext pra buscar e salvar no banco. 
// As buscas vão ao banco na hora. As mudanças (Adicionar e Remover) só ficam anotadas,
//  e são gravadas quando o service chama o SalvarAsync.
public class PratoRepositoryEf : IPratoRepository
{
    private readonly AppDbContext _context;

    public PratoRepositoryEf(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Prato>> ListarAsync()
    {
        return await _context.Pratos.ToListAsync();
    }

    public async Task<List<Prato>> ListarPorRestauranteAsync(int restauranteId)
    {
        return await _context.Pratos
            .Where(p => p.RestauranteId == restauranteId)
            .ToListAsync();
    }

    public async Task<Prato?> ObterAsync(int id)
    {
        return await _context.Pratos.FindAsync(id); //aqui como nao buscamos restaurnate junto pode ser o findasinc
    }

    public async Task<bool> ExisteAsync(string nome, int restauranteId, int? idIgnorado)
    {
        return await _context.Pratos.AnyAsync(p =>
            p.Nome == nome &&
            p.RestauranteId == restauranteId &&
            (idIgnorado == null || p.Id != idIgnorado));
    }

    public async Task<bool> RestauranteExisteAsync(int restauranteId) //ve se tem o dono do prato (é oq cruza o 1:N)
    {
        return await _context.Restaurantes.AnyAsync(r => r.Id == restauranteId);
    }

    public void Adicionar(Prato prato)
    {
        _context.Pratos.Add(prato);
    }

    public void Remover(Prato prato)
    {
        _context.Pratos.Remove(prato);
    }

    public async Task SalvarAsync()
    {
        await _context.SaveChangesAsync();
    }
}