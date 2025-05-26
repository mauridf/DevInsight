using DevInsight.Core.DTOs;
using DevInsight.Core.Exceptions;
using DevInsight.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevInsight.API.Controllers;

[Authorize]
[ApiController]
[Route("api/projetos/{projetoId}/documentos-links")]
public class DocumentoLinkController : ControllerBase
{
    private readonly IDocumentoLinkService _documentoLinkService;
    private readonly ILogger<DocumentoLinkController> _logger;

    public DocumentoLinkController(IDocumentoLinkService documentoLinkService, ILogger<DocumentoLinkController> logger)
    {
        _documentoLinkService = documentoLinkService;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> CriarDocumentoLink(Guid projetoId, [FromBody] DocumentoLinkCriacaoDTO documentoLinkDto)
    {
        try
        {
            var documentoLinkCriado = await _documentoLinkService.CriarDocumentoLinkAsync(documentoLinkDto, projetoId);
            return CreatedAtAction(nameof(ObterPorId), new { projetoId, id = documentoLinkCriado.Id }, documentoLinkCriado);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar DocumentoLink");
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ObterPorId(Guid projetoId, Guid id)
    {
        try
        {
            var documentoLink = await _documentoLinkService.ObterPorIdAsync(id);
            return Ok(documentoLink);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter DocumentoLink por ID: {DocumentoLinkId}", id);
            return StatusCode(500, new { message = "Ocorreu um erro interno" });
        }
    }

    [HttpGet]
    public async Task<IActionResult> ListarPorProjeto(Guid projetoId)
    {
        try
        {
            var documentosLinks = await _documentoLinkService.ListarPorProjetoAsync(projetoId);
            return Ok(documentosLinks);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar DocumentosLinks por projeto: {ProjetoId}", projetoId);
            return StatusCode(500, new { message = "Ocorreu um erro interno" });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> AtualizarDocumentoLink(Guid projetoId, Guid id, [FromBody] DocumentoLinkAtualizacaoDTO documentoLinkDto)
    {
        try
        {
            var documentoLinkAtualizado = await _documentoLinkService.AtualizarDocumentoLinkAsync(id, documentoLinkDto);
            return Ok(documentoLinkAtualizado);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar DocumentoLink: {DocumentoLinkId}", id);
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin,Consultor")]
    public async Task<IActionResult> ExcluirDocumentoLink(Guid projetoId, Guid id)
    {
        try
        {
            var resultado = await _documentoLinkService.ExcluirDocumentoLinkAsync(id);
            if (resultado)
                return NoContent();

            return BadRequest(new { message = "Não foi possível excluir o DocumentoLink" });
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao excluir DocumentoLink: {DocumentoLinkId}", id);
            return StatusCode(500, new { message = "Ocorreu um erro interno" });
        }
    }
}