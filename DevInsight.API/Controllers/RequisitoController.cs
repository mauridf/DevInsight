using DevInsight.Core.DTOs;
using DevInsight.Core.Exceptions;
using DevInsight.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevInsight.API.Controllers;

[Authorize]
[ApiController]
[Route("api/projetos/{projetoId}/requisitos")]
public class RequisitoController : ControllerBase
{
    private readonly IRequisitoService _requisitoService;
    private readonly ILogger<RequisitoController> _logger;

    public RequisitoController(IRequisitoService requisitoService, ILogger<RequisitoController> logger)
    {
        _requisitoService = requisitoService;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> CriarRequisito(Guid projetoId, [FromBody] RequisitoCriacaoDTO requisitoDto)
    {
        try
        {
            var requisitoCriado = await _requisitoService.CriarRequisitoAsync(requisitoDto, projetoId);
            return CreatedAtAction(nameof(ObterPorId), new { projetoId, id = requisitoCriado.Id }, requisitoCriado);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar requisito");
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ObterPorId(Guid projetoId, Guid id)
    {
        try
        {
            var requisito = await _requisitoService.ObterPorIdAsync(id);
            return Ok(requisito);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter requisito por ID: {RequisitoId}", id);
            return StatusCode(500, new { message = "Ocorreu um erro interno" });
        }
    }

    [HttpGet]
    public async Task<IActionResult> ListarPorProjeto(Guid projetoId)
    {
        try
        {
            var requisitos = await _requisitoService.ListarPorProjetoAsync(projetoId);
            return Ok(requisitos);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar requisitos por projeto: {ProjetoId}", projetoId);
            return StatusCode(500, new { message = "Ocorreu um erro interno" });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> AtualizarRequisito(Guid projetoId, Guid id, [FromBody] RequisitoAtualizacaoDTO requisitoDto)
    {
        try
        {
            var requisitoAtualizado = await _requisitoService.AtualizarRequisitoAsync(id, requisitoDto);
            return Ok(requisitoAtualizado);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar requisito: {RequisitoId}", id);
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin,Consultor")]
    public async Task<IActionResult> ExcluirRequisito(Guid projetoId, Guid id)
    {
        try
        {
            var resultado = await _requisitoService.ExcluirRequisitoAsync(id);
            if (resultado)
                return NoContent();

            return BadRequest(new { message = "Não foi possível excluir o requisito" });
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao excluir requisito: {RequisitoId}", id);
            return StatusCode(500, new { message = "Ocorreu um erro interno" });
        }
    }
}