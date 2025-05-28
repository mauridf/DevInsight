using DevInsight.Core.DTOs;
using DevInsight.Core.Exceptions;
using DevInsight.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevInsight.API.Controllers;

[Authorize(Roles = "Admin,Consultor")]
[ApiController]
[Route("api/projetos/{projetoId}/stakeholders")]
public class StakeHolderController : ControllerBase
{
    private readonly IStakeHolderService _stakeHolderService;
    private readonly ILogger<StakeHolderController> _logger;

    public StakeHolderController(IStakeHolderService stakeHolderService, ILogger<StakeHolderController> logger)
    {
        _stakeHolderService = stakeHolderService;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> CriarStakeHolder(Guid projetoId, [FromBody] StakeHolderCriacaoDTO stakeHolderDto)
    {
        try
        {
            var stakeHolderCriado = await _stakeHolderService.CriarStakeHolderAsync(stakeHolderDto, projetoId);
            return CreatedAtAction(nameof(ObterPorId), new { projetoId, id = stakeHolderCriado.Id }, stakeHolderCriado);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar StakeHolder");
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ObterPorId(Guid projetoId, Guid id)
    {
        try
        {
            var stakeHolder = await _stakeHolderService.ObterPorIdAsync(id);
            return Ok(stakeHolder);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter StakeHolder por ID: {StakeHolderId}", id);
            return StatusCode(500, new { message = "Ocorreu um erro interno" });
        }
    }

    [HttpGet]
    public async Task<IActionResult> ListarPorProjeto(Guid projetoId)
    {
        try
        {
            var stakeHolders = await _stakeHolderService.ListarPorProjetoAsync(projetoId);
            return Ok(stakeHolders);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar StakeHolders por projeto: {ProjetoId}", projetoId);
            return StatusCode(500, new { message = "Ocorreu um erro interno" });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> AtualizarStakeHolder(Guid projetoId, Guid id, [FromBody] StakeHolderAtualizacaoDTO stakeHolderDto)
    {
        try
        {
            var stakeHolderAtualizado = await _stakeHolderService.AtualizarStakeHolderAsync(id, stakeHolderDto);
            return Ok(stakeHolderAtualizado);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar StakeHolder: {StakeHolderId}", id);
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin,Consultor")]
    public async Task<IActionResult> ExcluirStakeHolder(Guid projetoId, Guid id)
    {
        try
        {
            var resultado = await _stakeHolderService.ExcluirStakeHolderAsync(id);
            if (resultado)
                return NoContent();

            return BadRequest(new { message = "Não foi possível excluir o StakeHolder" });
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao excluir StakeHolder: {StakeHolderId}", id);
            return StatusCode(500, new { message = "Ocorreu um erro interno" });
        }
    }
}