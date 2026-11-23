using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace APIRestaurante.Models;

public class Prato
{
    public int Id { get; set; }

    [Required(ErrorMessage = "O nome é obrigatório.")]
    public string Nome { get; set; } = string.Empty;

    public decimal Preco { get; set; }

    public bool Vegetariano { get; set; }

    // Chave estrangeira: o Id do restaurante dono deste prato
    public int RestauranteId { get; set; }

    // Propriedade de navegação: o restaurante dono deste prato
    [JsonIgnore]
    public Restaurante? Restaurante { get; set; }
}