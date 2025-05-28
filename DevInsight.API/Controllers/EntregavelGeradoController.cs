using System.Text;
using DevInsight.Core.DTOs;
using DevInsight.Core.Enums;
using DevInsight.Core.Exceptions;
using DevInsight.Core.Interfaces.Services;
using DevInsight.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevInsight.API.Controllers;

[Authorize]
[ApiController]
[Route("api/projetos/{projetoId}/entregaveis")]
public class EntregavelGeradoController : ControllerBase
{
    private readonly IEntregavelGeradoService _entregavelService;
    private readonly IDocumentGeneratorService _docService;
    private readonly IStorageService _storageService;
    private readonly ILogger<EntregavelGeradoController> _logger;

    public EntregavelGeradoController(
        IEntregavelGeradoService entregavelService,
        IDocumentGeneratorService docService,
        IStorageService storageService,
        ILogger<EntregavelGeradoController> logger)
    {
        _entregavelService = entregavelService;
        _docService = docService;
        _storageService = storageService;
        _logger = logger;
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Consultor")]
    public async Task<IActionResult> CriarEntregavel(Guid projetoId, [FromBody] EntregavelGeradoCriacaoDTO entregavelDto)
    {
        try
        {
            var entregavelCriado = await _entregavelService.CriarEntregavelAsync(entregavelDto, projetoId);
            return CreatedAtAction(nameof(ObterPorId), new { projetoId, id = entregavelCriado.Id }, entregavelCriado);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar entregável");
            return BadRequest(new { message = "Erro ao criar entregável" });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ObterPorId(Guid projetoId, Guid id)
    {
        try
        {
            var entregavel = await _entregavelService.ObterPorIdAsync(id);
            return Ok(entregavel);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter entregável por ID: {EntregavelId}", id);
            return StatusCode(500, new { message = "Ocorreu um erro interno" });
        }
    }

    [HttpGet]
    public async Task<IActionResult> ListarPorProjeto(Guid projetoId)
    {
        try
        {
            var entregaveis = await _entregavelService.ListarPorProjetoAsync(projetoId);
            return Ok(entregaveis);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar entregáveis por projeto: {ProjetoId}", projetoId);
            return StatusCode(500, new { message = "Ocorreu um erro interno" });
        }
    }

    [HttpPost("documento")]
    public async Task<IActionResult> CriarEntregavelComDocumento(Guid projetoId, [FromBody] GerarDocumentoDTO documentoDto)
    {
        try
        {
            byte[] fileContent;
            string fileName = $"doc_{DateTime.UtcNow:yyyyMMddHHmmss}";
            string contentType;

            switch (documentoDto.Formato)
            {
                case FormatoEntregavel.Pdf:
                    fileContent = await _docService.GeneratePdfAsync(documentoDto.Conteudo, documentoDto.Formato);
                    fileName += ".pdf";
                    contentType = "application/pdf";
                    break;

                case FormatoEntregavel.MarkDown:
                    var markdown = await _docService.GenerateMarkdownAsync(documentoDto.Conteudo);
                    fileContent = Encoding.UTF8.GetBytes(markdown);
                    fileName += ".md";
                    contentType = "text/markdown";
                    break;

                default:
                    return BadRequest("Formato de documento não suportado");
            }

            // Salvar no storage (S3 ou Local)
            using var stream = new MemoryStream(fileContent);
            var formFile = new FormFile(stream, 0, fileContent.Length, fileName, fileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = contentType
            };

            var path = await _storageService.UploadFileAsync(formFile, "documentos", fileName);
            var url = await _storageService.GetFileUrlAsync(path);

            return Ok(new EntregavelGeradoCriacaoDTO
            {
                NomeArquivo = fileName,
                UrlDownload = url,
                Formato = documentoDto.Formato
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao gerar documento");
            return StatusCode(500, "Erro ao gerar documento");
        }
    }

    [HttpGet("{id}/download")]
    public async Task<IActionResult> GerarUrlDownload(Guid projetoId, Guid id)
    {
        try
        {
            var url = await _entregavelService.GerarUrlDownloadAsync(id);
            return Ok(new { UrlDownload = url });
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao gerar URL de download: {EntregavelId}", id);
            return StatusCode(500, new { message = "Ocorreu um erro interno" });
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin,Consultor")]
    public async Task<IActionResult> ExcluirEntregavel(Guid projetoId, Guid id)
    {
        try
        {
            var resultado = await _entregavelService.ExcluirEntregavelAsync(id);
            if (resultado)
                return NoContent();

            return BadRequest(new { message = "Não foi possível excluir o entregável" });
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao excluir entregável: {EntregavelId}", id);
            return StatusCode(500, new { message = "Ocorreu um erro interno" });
        }
    }
}