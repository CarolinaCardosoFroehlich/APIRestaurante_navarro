using APIRestaurante.Models;
using APIRestaurante.Services;
using Microsoft.AspNetCore.Mvc;

namespace APIRestaurante.Controllers;


/// Recebe as requisições de restaurantes, 
/// chama o serviço e escolhe a resposta HTTP.


//O controller pede usando o _servico. 
// Ele só pode pedir o que está listado no IRestauranteService, 
// o pedido vai direto pro RestauranteService


[Route("api/[controller]")]
[ApiController]
public class RestaurantesController : ControllerBase
{
    private readonly IRestauranteService _servico; //cria a variável _servico, que vai guardar o RestauranteService.
    private readonly IPratoService _pratoServico;

    public RestaurantesController(IRestauranteService servico, IPratoService pratoServico)
    {
        _servico = servico;
        _pratoServico = pratoServico;
    }

    // GET: api/Restaurantes
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Restaurante>>> GetRestaurantes()
    {
        return Ok(await _servico.ListarAsync());
    }

    // GET: api/Restaurantes/X
    [HttpGet("{id}")]
    public async Task<ActionResult<Restaurante>> GetRestaurante(int id)
    {
        var restaurante = await _servico.ObterAsync(id); //pede a resposta pro service

        if (restaurante is null)
        {
            return NotFound(); 
        }

        return Ok(restaurante);
    }

    // GET: api/Restaurantes/X/Pratos
    [HttpGet("{id}/Pratos")]
    public async Task<ActionResult<IEnumerable<Prato>>> GetPratosDoRestaurante(int id)
    {
        var pratos = await _pratoServico.ListarPorRestauranteAsync(id);

        if (pratos is null)
        {
            return NotFound(); 
        }

        return Ok(pratos);
    }

    // POST: api/Restaurantes
    [HttpPost]
    public async Task<ActionResult<Restaurante>> PostRestaurante(Restaurante restaurante)
    {
        try
        {
            var criado = await _servico.CriarAsync(restaurante);
            return CreatedAtAction(nameof(GetRestaurante), new { id = criado.Id }, criado);
        }
        catch (RegraDeNegocioException ex)
        {
            return BadRequest(ex.Message); 
        }
    }

    // PUT: api/Restaurantes/X
    [HttpPut("{id}")]
    public async Task<IActionResult> PutRestaurante(int id, Restaurante restauranteAtualizado)
    {
        try
        {
            var atualizado = await _servico.AtualizarAsync(id, restauranteAtualizado);

            if (!atualizado)
            {
                return NotFound(); 
            }

            return NoContent();
        }
        catch (RegraDeNegocioException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // DELETE: api/Restaurantes/X
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRestaurante(int id)
    {
        try
        {
            var removido = await _servico.RemoverAsync(id);

            if (!removido)
            {
                return NotFound();
            }

            return NoContent();
        }
        catch (RegraDeNegocioException ex)
        {
            return BadRequest(ex.Message); 
        }
    }
}