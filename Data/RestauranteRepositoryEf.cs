using APIRestaurante.Models;
using APIRestaurante.Services;
using Microsoft.EntityFrameworkCore;

namespace APIRestaurante.Data;

//O repositório usa o AppDbContext pra buscar e salvar no banco. 
// As buscas vão ao banco na hora. As mudanças (Adicionar e Remover) só ficam anotadas,
// e são gravadas quando o service chama o SalvarAsync.
public class RestauranteRepositoryEf : IRestauranteRepository
{
    private readonly AppDbContext _context;


    //Pela injeção de dependência, o repositório recebe o AppDbContext no construtor, guarda na variável _context e usa ela pra buscar e salvar dados no banco.
    public RestauranteRepositoryEf(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Restaurante>> ListarAsync()
    {
        return await _context.Restaurantes //busca a tabela de restaurantes e traga os pratos de cada restaurante junto
            .ToListAsync();
    }

    public async Task<Restaurante?> ObterAsync(int id) //busca o restaurante cujo Id é igual ao número que você passou
    {
        return await _context.Restaurantes //aqui nao podemos usar findasync pq temos que buscar os pratos junto
            .Include(r => r.Pratos) //é oq cruza o (1:N)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<bool> ExisteAsync(string nome, string cidade, int? idIgnorado) //busca se existe algum restaurante que bate com a condição?". Devolve true ou false
    {
        return await _context.Restaurantes.AnyAsync(r =>
            r.Nome == nome &&
            r.Cidade == cidade &&
            (idIgnorado == null || r.Id != idIgnorado));
    }

    public async Task<bool> TemPratosAsync(int id) //busca n tabela  Pratos (_context.Pratos) se  existe algum prato cujo RestauranteId é esse número?
    {
        return await _context.Pratos.AnyAsync(p => p.RestauranteId == id);
    }


    //so anotam a mudanca, nada vai pro banco ainda 
    public void Adicionar(Restaurante restaurante)
    {
        _context.Restaurantes.Add(restaurante);
    }

    public void Remover(Restaurante restaurante)
    {
        _context.Restaurantes.Remove(restaurante);
    }

    public async Task SalvarAsync()
    {
        await _context.SaveChangesAsync();
    }
}