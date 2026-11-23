using APIRestaurante.Models;

namespace APIRestaurante.Services;

// o IRepository é a lista do que precisa ser buscado pro service obter as respostas que precisa

public interface IPratoRepository
{
    Task<List<Prato>> ListarAsync();

    // Só os pratos de um restaurante
    Task<List<Prato>> ListarPorRestauranteAsync(int restauranteId);

    Task<Prato?> ObterAsync(int id);

    // "Já existe um prato com esse nome NESTE restaurante?"
    Task<bool> ExisteAsync(string nome, int restauranteId, int? idIgnorado);

    // "O restaurante dono deste prato existe?"
    Task<bool> RestauranteExisteAsync(int restauranteId);

    void Adicionar(Prato prato);
    void Remover(Prato prato);
    Task SalvarAsync(); //salva tudo que foi alterado, adicionado ou removido
}