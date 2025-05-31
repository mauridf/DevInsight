using DevInsight.Core.DTOs;
using DevInsight.Core.Exceptions;
using DevInsight.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DevInsight.API.Controllers;

[Authorize]
[Route("api/projetos/{projetoId}/estimativascustos")]
[ApiController]
public class EstimativasCustosController : ControllerBase
{
    private readonly IEstimativaCusto _estimativaService;
    private readonly ILogger<EstimativasCustosController> _logger;

    public EstimativasCustosController(IEstimativaCusto estimativaService, ILogger<EstimativasCustosController> logger)
    {
        _estimativaService = estimativaService;
        _logger = logger;
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Consultor")]
    public async Task<IActionResult> CriarEstimativaCusto(Guid projetoId, [FromBody] EstimativaCustoCriacaoDTO estimativaDto)
    {
        try
        {
            var estimativa = await _estimativaService.CriarEstimativaCustoAsync(estimativaDto, projetoId);
            return CreatedAtAction(nameof(ObterPorId), new { projetoId, id = estimativa.Id }, estimativa);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar estimativa e custo do projeto");
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ObterPorId(Guid projetoId, Guid id)
    {
        try
        {
            var estimativa = await _estimativaService.ObterPorIdAsync(id);
            return Ok(estimativa);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter estimativa e custo por ID: {EstimativaId}", id);
            return StatusCode(500, new { message = "Ocorreu um erro interno" });
        }
    }

    [HttpGet]
    public async Task<IActionResult> ListarPorProjeto(Guid projetoId)
    {
        try
        {
            var estimativas = await _estimativaService.ListarPorProjetoAsync(projetoId);
            return Ok(estimativas);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar estimativas e custos por projeto: {ProjetoId}", projetoId);
            return StatusCode(500, new { message = "Ocorreu um erro interno" });
        }
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Consultor")]
    public async Task<IActionResult> AtualizarEstimativaCusto(Guid projetoId, Guid id, [FromBody] EstimativaCustoAtualizacaoDTO estimativaDto)
    {
        try
        {
            var estimativaAtualizada = await _estimativaService.AtualizarEstimativaCustoAsync(id, estimativaDto);
            return Ok(estimativaAtualizada);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar estimativa e custo: {FaseId}", id);
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin,Consultor")]
    public async Task<IActionResult> ExcluirEstimativaCusto(Guid projetoId, Guid id)
    {
        try
        {
            var estimativa = await _estimativaService.ExcluirEstimativaCustoAsync(id);
            if (estimativa)
                return NoContent();

            return BadRequest(new { message = "Não foi possível excluir a estimativa e custo do projeto" });
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao excluir estimativa e custo do projeto: {EstimativaId}", id);
            return StatusCode(500, new { message = "Ocorreu um erro interno" });
        }
    }
}
