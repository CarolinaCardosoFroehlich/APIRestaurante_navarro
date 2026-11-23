using APIRestaurante.Models;

namespace APIRestaurante.Services;

//a lista do que o controller pode pedir 
public interface IPratoService
{
    Task<List<Prato>> ListarAsync();

    // null = o restaurante não existe
    Task<List<Prato>?> ListarPorRestauranteAsync(int restauranteId);

    Task<Prato?> ObterAsync(int id);
    Task<Prato> CriarAsync(Prato prato);
    Task<bool> AtualizarAsync(int id, Prato pratoAtualizado);
    Task<bool> RemoverAsync(int id);
}