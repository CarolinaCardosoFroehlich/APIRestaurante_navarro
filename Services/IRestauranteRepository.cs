using APIRestaurante.Models;

namespace APIRestaurante.Services;

// o IRepository é a lista do que precisa ser buscado pro service obter as respostas que precisa

public interface IRestauranteRepository
{
    Task<List<Restaurante>> ListarAsync(); //o task é usado pq falar com o banco demora entao isso roda depois de promisse 
    Task<Restaurante?> ObterAsync(int id);

    // "Já existe um restaurante com esse nome nessa cidade?"
    Task<bool> ExisteAsync(string nome, string cidade, int? idIgnorado);

    // "Este restaurante ainda tem pratos?" 
    Task<bool> TemPratosAsync(int id);

    void Adicionar(Restaurante restaurante);
    void Remover(Restaurante restaurante);

    Task SalvarAsync(); //salva tudo que foi alterado, adicionado ou removido
}