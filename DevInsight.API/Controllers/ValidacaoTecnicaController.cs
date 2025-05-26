using DevInsight.Core.DTOs;
using DevInsight.Core.Exceptions;
using DevInsight.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevInsight.API.Controllers;

[Authorize]
[ApiController]
[Route("api/projetos/{projetoId}/validacoes-tecnicas")]
public class ValidacaoTecnicaController : ControllerBase
{
    private readonly IValidacaoTecnicaService _validacaoService;
    private readonly ILogger<ValidacaoTecnicaController> _logger;

    public ValidacaoTecnicaController(IValidacaoTecnicaService validacaoService, ILogger<ValidacaoTecnicaController> logger)
    {
        _validacaoService = validacaoService;
        _logger = logger;
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Consultor")]
    public async Task<IActionResult> CriarValidacao(Guid projetoId, [FromBody] ValidacaoTecnicaCriacaoDTO validacaoDto)
    {
        try
        {
            var validacaoCriada = await _validacaoService.CriarValidacaoAsync(validacaoDto, projetoId);
            return CreatedAtAction(nameof(ObterPorId), new { projetoId, id = validacaoCriada.Id }, validacaoCriada);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar validação técnica");
            return BadRequest(new { message = "Erro ao criar validação técnica" });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ObterPorId(Guid projetoId, Guid id)
    {
        try
        {
            var validacao = await _validacaoService.ObterPorIdAsync(id);
            return Ok(validacao);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter validação técnica por ID: {ValidacaoId}", id);
            return StatusCode(500, new { message = "Ocorreu um erro interno" });
        }
    }

    [HttpGet]
    public async Task<IActionResult> ListarPorProjeto(Guid projetoId)
    {
        try
        {
            var validacoes = await _validacaoService.ListarPorProjetoAsync(projetoId);
            return Ok(validacoes);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar validações técnicas por projeto: {ProjetoId}", projetoId);
            return StatusCode(500, new { message = "Ocorreu um erro interno" });
        }
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Consultor")]
    public async Task<IActionResult> AtualizarValidacao(Guid projetoId, Guid id, [FromBody] ValidacaoTecnicaAtualizacaoDTO validacaoDto)
    {
        try
        {
            var validacaoAtualizada = await _validacaoService.AtualizarValidacaoAsync(id, validacaoDto);
            return Ok(validacaoAtualizada);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar validação técnica: {ValidacaoId}", id);
            return BadRequest(new { message = "Erro ao atualizar validação técnica" });
        }
    }

    [HttpPatch("{id}/validar")]
    [Authorize(Roles = "Admin,Consultor")]
    public async Task<IActionResult> MarcarComoValidado(Guid projetoId, Guid id, [FromBody] string? observacao)
    {
        try
        {
            var validacaoAtualizada = await _validacaoService.MarcarComoValidadoAsync(id, observacao);
            return Ok(validacaoAtualizada);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao marcar validação técnica como validada: {ValidacaoId}", id);
            return BadRequest(new { message = "Erro ao marcar validação técnica como validada" });
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin,Consultor")]
    public async Task<IActionResult> ExcluirValidacao(Guid projetoId, Guid id)
    {
        try
        {
            var resultado = await _validacaoService.ExcluirValidacaoAsync(id);
            if (resultado)
                return NoContent();

            return BadRequest(new { message = "Não foi possível excluir a validação técnica" });
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao excluir validação técnica: {ValidacaoId}", id);
            return StatusCode(500, new { message = "Ocorreu um erro interno" });
        }
    }
}