using DevInsight.API.Controllers;
using DevInsight.Core.DTOs;
using DevInsight.Core.Exceptions;
using DevInsight.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevInsight.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/projetos/{projetoId}/solucoes-propostas")]
public class SolucaoPropostaController : ControllerBase
{
    private readonly ISolucaoPropostaService _solucaoPropostaService;
    private readonly ILogger<SolucaoPropostaController> _logger;

    public SolucaoPropostaController(ISolucaoPropostaService solucaoPropostaService, ILogger<SolucaoPropostaController> logger)
    {
        _solucaoPropostaService = solucaoPropostaService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> ListarPorProjeto(Guid projetoId)
    {
        try
        {
            var solucoes = await _solucaoPropostaService.ListarPorProjetoAsync(projetoId);
            return Ok(solucoes);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar soluções por projeto: {ProjetoId}", projetoId);
            return StatusCode(500, new { message = "Ocorreu um erro interno" });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ObterPorId(Guid projetoId, Guid id)
    {
        try
        {
            var solucao = await _solucaoPropostaService.ObterPorIdAsync(id);
            return Ok(solucao);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter solução por ID: {SolucaoId}", id);
            return StatusCode(500, new { message = "Ocorreu um erro interno" });
        }
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Consultor")]
    public async Task<IActionResult> Criar(Guid projetoId, [FromBody] SolucaoPropostaCriacaoDTO dto)
    {
        try
        {
            var solucao = await _solucaoPropostaService.CriarAsync(projetoId, dto);
            return CreatedAtAction(nameof(ObterPorId), new { projetoId, id = solucao.Id }, solucao);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar solução");
            return BadRequest(new { message = "Erro ao criar solução" });
        }
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Consultor")]
    public async Task<IActionResult> Atualizar(Guid projetoId, Guid id, [FromBody] SolucaoPropostaAtualizacaoDTO dto)
    {
        try
        {
            var solucao = await _solucaoPropostaService.AtualizarAsync(id, dto);
            return Ok(solucao);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar solução: {SolucaoId}", id);
            return BadRequest(new { message = "Erro ao atualizar solução" });
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin,Consultor")]
    public async Task<IActionResult> Excluir(Guid projetoId, Guid id)
    {
        try
        {
            await _solucaoPropostaService.ExcluirAsync(id);
            return NoContent();
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao excluir solução: {SolucaoId}", id);
            return StatusCode(500, new { message = "Ocorreu um erro interno" });
        }
    }
}