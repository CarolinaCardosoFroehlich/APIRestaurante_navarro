using APIRestaurante.Models;
using APIRestaurante.Services;
using Microsoft.AspNetCore.Mvc;

namespace APIRestaurante.Controllers;

// Recebe as requisições de pratos, chama o serviço e escolhe a resposta HTTP.

[Route("api/[controller]")]
[ApiController]
public class PratosController : ControllerBase
{
    private readonly IPratoService _servico;

    public PratosController(IPratoService servico)
    {
        _servico = servico;
    }

    // GET: api/Pratos
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Prato>>> GetPratos()
    {
        return Ok(await _servico.ListarAsync());
    }

    // GET: api/Pratos/X
    [HttpGet("{id}")]
    public async Task<ActionResult<Prato>> GetPrato(int id)
    {
        var prato = await _servico.ObterAsync(id);

        if (prato is null)
        {
            return NotFound(); 
        }

        return Ok(prato);
    }

    // POST: api/Pratos
    [HttpPost]
    public async Task<ActionResult<Prato>> PostPrato(Prato prato)
    {
        try
        {
            var criado = await _servico.CriarAsync(prato);
            return CreatedAtAction(nameof(GetPrato), new { id = criado.Id }, criado);
        }
        catch (RegraDeNegocioException ex)
        {
            return BadRequest(ex.Message); 
        }
    }

    // PUT: api/Pratos/X
    [HttpPut("{id}")]
    public async Task<IActionResult> PutPrato(int id, Prato pratoAtualizado)
    {
        try
        {
            var atualizado = await _servico.AtualizarAsync(id, pratoAtualizado);

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

    // DELETE: api/Pratos/X
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePrato(int id)
    {
        var removido = await _servico.RemoverAsync(id);

        if (!removido)
        {
            return NotFound();
        }

        return NoContent();
    }
}