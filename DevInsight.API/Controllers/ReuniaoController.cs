using DevInsight.Core.DTOs;
using DevInsight.Core.Exceptions;
using DevInsight.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevInsight.API.Controllers;

[Authorize]
[ApiController]
[Route("api/projetos/{projetoId}/reunioes")]
public class ReuniaoController : ControllerBase
{
    private readonly IReuniaoService _reuniaoService;
    private readonly ILogger<ReuniaoController> _logger;

    public ReuniaoController(IReuniaoService reuniaoService, ILogger<ReuniaoController> logger)
    {
        _reuniaoService = reuniaoService;
        _logger = logger;
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Consultor")]
    public async Task<IActionResult> CriarReuniao(Guid projetoId, [FromBody] ReuniaoCriacaoDTO reuniaoDto)
    {
        try
        {
            var reuniaoCriada = await _reuniaoService.CriarReuniaoAsync(reuniaoDto, projetoId);
            return CreatedAtAction(nameof(ObterPorId), new { projetoId, id = reuniaoCriada.Id }, reuniaoCriada);
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
            _logger.LogError(ex, "Erro ao criar reunião");
            return BadRequest(new { message = "Erro ao criar reunião" });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ObterPorId(Guid projetoId, Guid id)
    {
        try
        {
            var reuniao = await _reuniaoService.ObterPorIdAsync(id);
            return Ok(reuniao);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter reunião por ID: {ReuniaoId}", id);
            return StatusCode(500, new { message = "Ocorreu um erro interno" });
        }
    }

    [HttpGet]
    public async Task<IActionResult> ListarPorProjeto(Guid projetoId)
    {
        try
        {
            var reunioes = await _reuniaoService.ListarPorProjetoAsync(projetoId);
            return Ok(reunioes);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar reuniões por projeto: {ProjetoId}", projetoId);
            return StatusCode(500, new { message = "Ocorreu um erro interno" });
        }
    }

    [HttpGet("proximas")]
    public async Task<IActionResult> ListarProximasReunioes([FromQuery] int dias = 7)
    {
        try
        {
            var reunioes = await _reuniaoService.ListarProximasReunioesAsync(dias);
            return Ok(reunioes);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar próximas reuniões");
            return StatusCode(500, new { message = "Ocorreu um erro interno" });
        }
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Consultor")]
    public async Task<IActionResult> AtualizarReuniao(Guid projetoId, Guid id, [FromBody] ReuniaoAtualizacaoDTO reuniaoDto)
    {
        try
        {
            var reuniaoAtualizada = await _reuniaoService.AtualizarReuniaoAsync(id, reuniaoDto);
            return Ok(reuniaoAtualizada);
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
            _logger.LogError(ex, "Erro ao atualizar reunião: {ReuniaoId}", id);
            return BadRequest(new { message = "Erro ao atualizar reunião" });
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin,Consultor")]
    public async Task<IActionResult> ExcluirReuniao(Guid projetoId, Guid id)
    {
        try
        {
            var resultado = await _reuniaoService.ExcluirReuniaoAsync(id);
            if (resultado)
                return NoContent();

            return BadRequest(new { message = "Não foi possível excluir a reunião" });
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao excluir reunião: {ReuniaoId}", id);
            return StatusCode(500, new { message = "Ocorreu um erro interno" });
        }
    }
}