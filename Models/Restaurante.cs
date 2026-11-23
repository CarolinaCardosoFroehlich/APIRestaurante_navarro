using System.ComponentModel.DataAnnotations;

namespace APIRestaurante.Models;

public class Restaurante
{
    public int Id { get; set; }

    [Required(ErrorMessage = "O nome é obrigatório.")]
    public string Nome { get; set; } = string.Empty;

    public string Cidade { get; set; } = string.Empty;

    public string TipoCozinha { get; set; } = string.Empty;

    // Propriedade de navegação: os pratos deste restaurante.
    // Não vira coluna no banco; o EF preenche lendo o RestauranteId dos pratos.
    public List<Prato> Pratos { get; set; } = [];
}
