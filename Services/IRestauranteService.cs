using APIRestaurante.Models;

namespace APIRestaurante.Services;

//a lista do que o controller pode pedir 

public interface IRestauranteService
{
    Task<List<Restaurante>> ListarAsync();
    Task<Restaurante?> ObterAsync(int id);
    Task<Restaurante> CriarAsync(Restaurante restaurante);
    Task<bool> AtualizarAsync(int id, Restaurante restauranteAtualizado);
    Task<bool> RemoverAsync(int id);
}