using DevInsight.Core.DTOs;
using DevInsight.Core.Exceptions;
using DevInsight.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevInsight.API.Controllers;

[Authorize]
[ApiController]
[Route("api/projetos/{projetoId}/entregas-finais")]
public class EntregaFinalController : ControllerBase
{
    private readonly IEntregaFinalService _entregaService;
    private readonly ILogger<EntregaFinalController> _logger;

    public EntregaFinalController(IEntregaFinalService entregaService, ILogger<EntregaFinalController> logger)
    {
        _entregaService = entregaService;
        _logger = logger;
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Consultor")]
    public async Task<IActionResult> CriarEntrega(Guid projetoId, [FromBody] EntregaFinalCriacaoDTO entregaDto)
    {
        try
        {
            var entregaCriada = await _entregaService.CriarEntregaAsync(entregaDto, projetoId);
            return CreatedAtAction(nameof(ObterPorId), new { projetoId, id = entregaCriada.Id }, entregaCriada);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar entrega final");
            return BadRequest(new { message = "Erro ao criar entrega final" });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ObterPorId(Guid projetoId, Guid id)
    {
        try
        {
            var entrega = await _entregaService.ObterPorIdAsync(id);
            return Ok(entrega);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter entrega final por ID: {EntregaId}", id);
            return StatusCode(500, new { message = "Ocorreu um erro interno" });
        }
    }

    [HttpGet]
    public async Task<IActionResult> ListarPorProjeto(Guid projetoId)
    {
        try
        {
            var entregas = await _entregaService.ListarPorProjetoAsync(projetoId);
            return Ok(entregas);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar entregas finais por projeto: {ProjetoId}", projetoId);
            return StatusCode(500, new { message = "Ocorreu um erro interno" });
        }
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Consultor")]
    public async Task<IActionResult> AtualizarEntrega(Guid projetoId, Guid id, [FromBody] EntregaFinalAtualizacaoDTO entregaDto)
    {
        try
        {
            var entregaAtualizada = await _entregaService.AtualizarEntregaAsync(id, entregaDto);
            return Ok(entregaAtualizada);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar entrega final: {EntregaId}", id);
            return BadRequest(new { message = "Erro ao atualizar entrega final" });
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin,Consultor")]
    public async Task<IActionResult> ExcluirEntrega(Guid projetoId, Guid id)
    {
        try
        {
            var resultado = await _entregaService.ExcluirEntregaAsync(id);
            if (resultado)
                return NoContent();

            return BadRequest(new { message = "Não foi possível excluir a entrega final" });
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao excluir entrega final: {EntregaId}", id);
            return StatusCode(500, new { message = "Ocorreu um erro interno" });
        }
    }
}