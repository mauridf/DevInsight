using DevInsight.Core.DTOs;
using DevInsight.Core.Exceptions;
using DevInsight.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DevInsight.API.Controllers;

[Authorize]
[Route("api/projetos/{projetoId}/fasesprojeto")]
[ApiController]
public class FaseProjetoController : ControllerBase
{
    private readonly IFaseProjetoService _faseService;
    private readonly ILogger<FaseProjetoController> _logger;

    public FaseProjetoController(IFaseProjetoService faseService, ILogger<FaseProjetoController> logger)
    {
        _faseService = faseService;
        _logger = logger;
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Consultor")]
    public async Task<IActionResult> CriarFaseProjeto(Guid projetoId, [FromBody] FaseProjetoCriacaoDTO faseDto)
    {
        try
        {
            var faseProjeto = await _faseService.CriarFaseProjetoAsync(faseDto, projetoId);
            return CreatedAtAction(nameof(ObterPorId), new { projetoId, id = faseProjeto.Id }, faseProjeto);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar fase do projeto");
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ObterPorId(Guid projetoId, Guid id)
    {
        try
        {
            var fase = await _faseService.ObterPorIdAsync(id);
            return Ok(fase);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter fase do projeto por ID: {PersonaId}", id);
            return StatusCode(500, new { message = "Ocorreu um erro interno" });
        }
    }

    [HttpGet]
    public async Task<IActionResult> ListarPorProjeto(Guid projetoId)
    {
        try
        {
            var fases = await _faseService.ListarPorProjetoAsync(projetoId);
            return Ok(fases);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar fases por projeto: {ProjetoId}", projetoId);
            return StatusCode(500, new { message = "Ocorreu um erro interno" });
        }
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Consultor")]
    public async Task<IActionResult> AtualizarFaseProjeto(Guid projetoId, Guid id, [FromBody] FaseProjetoAtualizacaoDTO faseDto)
    {
        try
        {
            var faseAtualizada = await _faseService.AtualizarFaseProjetoAsync(id, faseDto);
            return Ok(faseAtualizada);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar fase do projeto: {FaseId}", id);
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin,Consultor")]
    public async Task<IActionResult> ExcluirFaseProjeto(Guid projetoId, Guid id)
    {
        try
        {
            var fase = await _faseService.ExcluirFaseProjetoAsync(id);
            if (fase)
                return NoContent();

            return BadRequest(new { message = "Não foi possível excluir a fase do projeto" });
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao excluir fase do projeto: {PersonaId}", id);
            return StatusCode(500, new { message = "Ocorreu um erro interno" });
        }
    }
}
