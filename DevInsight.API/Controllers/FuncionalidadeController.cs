using DevInsight.Core.DTOs;
using DevInsight.Core.Exceptions;
using DevInsight.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevInsight.API.Controllers;

[Authorize]
[ApiController]
[Route("api/projetos/{projetoId}/funcionalidades")]
public class FuncionalidadeController : ControllerBase
{
    private readonly IFuncionalidadeService _funcionalidadeService;
    private readonly ILogger<FuncionalidadeController> _logger;

    public FuncionalidadeController(IFuncionalidadeService funcionalidadeService, ILogger<FuncionalidadeController> logger)
    {
        _funcionalidadeService = funcionalidadeService;
        _logger = logger;
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Consultor")]
    public async Task<IActionResult> CriarFuncionalidade(Guid projetoId, [FromBody] FuncionalidadeCriacaoDTO funcionalidadeDto)
    {
        try
        {
            var funcionalidadeCriada = await _funcionalidadeService.CriarFuncionalidadeAsync(funcionalidadeDto, projetoId);
            return CreatedAtAction(nameof(ObterPorId), new { projetoId, id = funcionalidadeCriada.Id }, funcionalidadeCriada);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar funcionalidade");
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ObterPorId(Guid projetoId, Guid id)
    {
        try
        {
            var funcionalidade = await _funcionalidadeService.ObterPorIdAsync(id);
            return Ok(funcionalidade);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter funcionalidade por ID: {FuncionalidadeId}", id);
            return StatusCode(500, new { message = "Ocorreu um erro interno" });
        }
    }

    [HttpGet]
    public async Task<IActionResult> ListarPorProjeto(Guid projetoId)
    {
        try
        {
            var funcionalidades = await _funcionalidadeService.ListarPorProjetoAsync(projetoId);
            return Ok(funcionalidades);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar funcionalidades por projeto: {ProjetoId}", projetoId);
            return StatusCode(500, new { message = "Ocorreu um erro interno" });
        }
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Consultor")]
    public async Task<IActionResult> AtualizarFuncionalidade(Guid projetoId, Guid id, [FromBody] FuncionalidadeAtualizacaoDTO funcionalidadeDto)
    {
        try
        {
            var funcionalidadeAtualizada = await _funcionalidadeService.AtualizarFuncionalidadeAsync(id, funcionalidadeDto);
            return Ok(funcionalidadeAtualizada);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar funcionalidade: {FuncionalidadeId}", id);
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin,Consultor")]
    public async Task<IActionResult> ExcluirFuncionalidade(Guid projetoId, Guid id)
    {
        try
        {
            var resultado = await _funcionalidadeService.ExcluirFuncionalidadeAsync(id);
            if (resultado)
                return NoContent();

            return BadRequest(new { message = "Não foi possível excluir a funcionalidade" });
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao excluir funcionalidade: {FuncionalidadeId}", id);
            return StatusCode(500, new { message = "Ocorreu um erro interno" });
        }
    }
}