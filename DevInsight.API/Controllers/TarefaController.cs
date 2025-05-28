using DevInsight.Core.DTOs;
using DevInsight.Core.Enums;
using DevInsight.Core.Exceptions;
using DevInsight.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevInsight.API.Controllers;

[Authorize]
[ApiController]
[Route("api/projetos/{projetoId}/tarefas")]
public class TarefaController : ControllerBase
{
    private readonly ITarefaService _tarefaService;
    private readonly ILogger<TarefaController> _logger;

    public TarefaController(ITarefaService tarefaService, ILogger<TarefaController> logger)
    {
        _tarefaService = tarefaService;
        _logger = logger;
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Consultor")]
    public async Task<IActionResult> CriarTarefa(Guid projetoId, [FromBody] TarefaCriacaoDTO tarefaDto)
    {
        try
        {
            var tarefaCriada = await _tarefaService.CriarTarefaAsync(tarefaDto, projetoId);
            return CreatedAtAction(nameof(ObterPorId), new { projetoId, id = tarefaCriada.Id }, tarefaCriada);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (BusinessException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar tarefa");
            return BadRequest(new { message = "Erro ao criar tarefa" });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ObterPorId(Guid projetoId, Guid id)
    {
        try
        {
            var tarefa = await _tarefaService.ObterPorIdAsync(id);
            return Ok(tarefa);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter tarefa por ID: {TarefaId}", id);
            return StatusCode(500, new { message = "Ocorreu um erro interno" });
        }
    }

    [HttpGet]
    public async Task<IActionResult> ListarPorProjeto(Guid projetoId)
    {
        try
        {
            var tarefas = await _tarefaService.ListarPorProjetoAsync(projetoId);
            return Ok(tarefas);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar tarefas por projeto: {ProjetoId}", projetoId);
            return StatusCode(500, new { message = "Ocorreu um erro interno" });
        }
    }

    [HttpGet("kanban")]
    public async Task<IActionResult> ListarParaKanban(Guid projetoId)
    {
        try
        {
            var tarefas = await _tarefaService.ListarParaKanbanAsync(projetoId);
            return Ok(tarefas);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar tarefas para Kanban: {ProjetoId}", projetoId);
            return StatusCode(500, new { message = "Ocorreu um erro interno" });
        }
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Consultor")]
    public async Task<IActionResult> AtualizarTarefa(Guid projetoId, Guid id, [FromBody] TarefaAtualizacaoDTO tarefaDto)
    {
        try
        {
            var tarefaAtualizada = await _tarefaService.AtualizarTarefaAsync(id, tarefaDto);
            return Ok(tarefaAtualizada);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (BusinessException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar tarefa: {TarefaId}", id);
            return BadRequest(new { message = "Erro ao atualizar tarefa" });
        }
    }

    [HttpPatch("{id}/status")]
    [Authorize(Roles = "Admin,Consultor")]
    public async Task<IActionResult> AtualizarStatus(Guid projetoId, Guid id, [FromBody] StatusTarefa status)
    {
        try
        {
            var tarefaAtualizada = await _tarefaService.AtualizarStatusAsync(id, status);
            return Ok(tarefaAtualizada);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar status da tarefa: {TarefaId}", id);
            return BadRequest(new { message = "Erro ao atualizar status da tarefa" });
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin,Consultor")]
    public async Task<IActionResult> ExcluirTarefa(Guid projetoId, Guid id)
    {
        try
        {
            var resultado = await _tarefaService.ExcluirTarefaAsync(id);
            if (resultado)
                return NoContent();

            return BadRequest(new { message = "Não foi possível excluir a tarefa" });
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao excluir tarefa: {TarefaId}", id);
            return StatusCode(500, new { message = "Ocorreu um erro interno" });
        }
    }
}