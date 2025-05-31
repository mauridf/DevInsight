using DevInsight.Core.DTOs;
using DevInsight.Core.Exceptions;
using DevInsight.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevInsight.API.Controllers;

[Authorize]
[Route("api/projetos/{projetoId}/personaschaves")]
[ApiController]
public class PersonaChaveController : ControllerBase
{
    private readonly IPersonaChave _personaService;
    private readonly ILogger<PersonaChaveController> _logger;

    public PersonaChaveController(IPersonaChave personaService, ILogger<PersonaChaveController> logger)
    {
        _personaService = personaService;
        _logger = logger;
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Consultor")]
    public async Task<IActionResult> CriarPersonaChave(Guid projetoId, [FromBody] PersonaChaveCriacaoDTO personaDto)
    {
        try
        {
            var personaCriada = await _personaService.CriarPersonaAsync(personaDto, projetoId);
            return CreatedAtAction(nameof(ObterPorId), new { projetoId, id = personaCriada.Id }, personaCriada);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar persona chave");
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ObterPorId(Guid projetoId, Guid id)
    {
        try
        {
            var persona = await _personaService.ObterPorIdAsync(id);
            return Ok(persona);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter persona chave por ID: {PersonaId}", id);
            return StatusCode(500, new { message = "Ocorreu um erro interno" });
        }
    }

    [HttpGet]
    public async Task<IActionResult> ListarPorProjeto(Guid projetoId)
    {
        try
        {
            var personas = await _personaService.ListarPorProjetoAsync(projetoId);
            return Ok(personas);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar personas por projeto: {ProjetoId}", projetoId);
            return StatusCode(500, new { message = "Ocorreu um erro interno" });
        }
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Consultor")]
    public async Task<IActionResult> AtualizarPersonaChave(Guid projetoId, Guid id, [FromBody] PersonaChaveAtualizacaoDTO personaDto)
    {
        try
        {
            var personaAtualizada = await _personaService.AtualizarPersonaAsync(id, personaDto);
            return Ok(personaAtualizada);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar persona chave: {PersonaId}", id);
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin,Consultor")]
    public async Task<IActionResult> ExcluirPersonaChave(Guid projetoId, Guid id)
    {
        try
        {
            var resultado = await _personaService.ExcluirPersonaAsync(id);
            if (resultado)
                return NoContent();

            return BadRequest(new { message = "Não foi possível excluir a persona chave" });
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao excluir persona chave: {PersonaId}", id);
            return StatusCode(500, new { message = "Ocorreu um erro interno" });
        }
    }
}
