using APIRestaurante.Models;

namespace APIRestaurante.Services;

/*/O RestauranteService vê se é possível fazer a ação. 
Se não achar o restaurante, devolve false ou null. 
Se uma regra for quebrada, lança um erro. Se der certo, devolve true ou o restaurante. 
O controller recebe isso e decide a resposta HTTP./*/
public class PratoService : IPratoService
{
    private readonly IPratoRepository _repositorio;

    public PratoService(IPratoRepository repositorio)
    {
        _repositorio = repositorio;
    }

    public async Task<List<Prato>> ListarAsync()
    {
        return await _repositorio.ListarAsync();
    }

    public async Task<List<Prato>?> ListarPorRestauranteAsync(int restauranteId)
    {
        if (!await _repositorio.RestauranteExisteAsync(restauranteId))
        {
            return null;
        }

        return await _repositorio.ListarPorRestauranteAsync(restauranteId);
    }

    public async Task<Prato?> ObterAsync(int id)
    {
        return await _repositorio.ObterAsync(id);
    }

    public async Task<Prato> CriarAsync(Prato prato)
    {
        await ValidarRegrasAsync(prato, idIgnorado: null);

        prato.Id = 0;

        _repositorio.Adicionar(prato);
        await _repositorio.SalvarAsync();

        return prato;
    }

    public async Task<bool> AtualizarAsync(int id, Prato pratoAtualizado)
    {
        var prato = await _repositorio.ObterAsync(id);

        if (prato is null)
        {
            return false;
        }

        await ValidarRegrasAsync(pratoAtualizado, idIgnorado: id);

        prato.Nome = pratoAtualizado.Nome;
        prato.Preco = pratoAtualizado.Preco;
        prato.Vegetariano = pratoAtualizado.Vegetariano;
        prato.RestauranteId = pratoAtualizado.RestauranteId;

        await _repositorio.SalvarAsync();

        return true;
    }

    public async Task<bool> RemoverAsync(int id)
    {
        var prato = await _repositorio.ObterAsync(id);

        if (prato is null)
        {
            return false;
        }

        _repositorio.Remover(prato);
        await _repositorio.SalvarAsync();

        return true;
    }

    private async Task ValidarRegrasAsync(Prato prato, int? idIgnorado)
    {
        // Regra 1: o restaurante dono do prato tem que existir
        if (!await _repositorio.RestauranteExisteAsync(prato.RestauranteId))
        {
            throw new RegraDeNegocioException("O restaurante informado não existe.");
        }

        // Regra 2: não pode ter dois pratos com o mesmo nome no mesmo restaurante
        if (await _repositorio.ExisteAsync(prato.Nome, prato.RestauranteId, idIgnorado))
        {
            throw new RegraDeNegocioException("Esse restaurante já tem um prato com esse nome.");
        }
    }
}