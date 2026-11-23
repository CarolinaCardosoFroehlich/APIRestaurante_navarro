using APIRestaurante.Models;

namespace APIRestaurante.Services;

/*/O RestauranteService vê se é possível fazer a ação. 
Se não achar o restaurante, devolve false ou null. 
Se uma regra for quebrada, lança um erro. Se der certo, devolve true ou o restaurante. 
O controller recebe isso e decide a resposta HTTP./*/
public class RestauranteService : IRestauranteService
{
    private readonly IRestauranteRepository _repositorio;

    public RestauranteService(IRestauranteRepository repositorio)
    {
        _repositorio = repositorio;
    }

    public async Task<List<Restaurante>> ListarAsync()
    {
        return await _repositorio.ListarAsync();
    }

    public async Task<Restaurante?> ObterAsync(int id)
    {
        return await _repositorio.ObterAsync(id);
    }

    public async Task<Restaurante> CriarAsync(Restaurante restaurante)
    {
        await ValidarRegrasAsync(restaurante, idIgnorado: null);

        restaurante.Id = 0;
        restaurante.Pratos = [];

        _repositorio.Adicionar(restaurante);
        await _repositorio.SalvarAsync();

        return restaurante;
    }

    public async Task<bool> AtualizarAsync(int id, Restaurante restauranteAtualizado)
    {
        var restaurante = await _repositorio.ObterAsync(id);

        if (restaurante is null)
        {
            return false;
        }

        await ValidarRegrasAsync(restauranteAtualizado, idIgnorado: id);

        restaurante.Nome = restauranteAtualizado.Nome;
        restaurante.Cidade = restauranteAtualizado.Cidade;
        restaurante.TipoCozinha = restauranteAtualizado.TipoCozinha;

        await _repositorio.SalvarAsync();

        return true;
    }

    public async Task<bool> RemoverAsync(int id)
    {
        var restaurante = await _repositorio.ObterAsync(id);

        if (restaurante is null)
        {
            return false;
        }

        // Regra 2: não pode apagar restaurante que ainda tem pratos
        if (await _repositorio.TemPratosAsync(id))
        {
            throw new RegraDeNegocioException(
                "Não é possível remover um restaurante que ainda tem pratos.");
        }

        _repositorio.Remover(restaurante);
        await _repositorio.SalvarAsync();

        return true;
    }

    private async Task ValidarRegrasAsync(Restaurante restaurante, int? idIgnorado)
    {
        // Regra 1: não pode ter dois restaurantes com o mesmo nome na mesma cidade
        if (await _repositorio.ExisteAsync(restaurante.Nome, restaurante.Cidade, idIgnorado))
        {
            throw new RegraDeNegocioException(
                "Já existe um restaurante com esse nome nessa cidade.");
        }
    }
}