using System.Text;
using DevInsight.Core.DTOs;
using DevInsight.Core.Enums;
using DevInsight.Core.Exceptions;
using DevInsight.Core.Interfaces.Services;
using DevInsight.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DevInsight.Core.Extensions;
using DevInsight.Core.Entities;

namespace DevInsight.API.Controllers;

[Authorize]
[ApiController]
[Route("api/projetos/{projetoId}/entregaveis")]
public class EntregavelGeradoController : ControllerBase
{
    private readonly IEntregavelGeradoService _entregavelService;
    private readonly IProjetoService _projetoService;
    private readonly IStorageService _storageService;
    private readonly ILogger<EntregavelGeradoController> _logger;

    public EntregavelGeradoController(
        IEntregavelGeradoService entregavelService,
        IProjetoService projetoService,
        IStorageService storageService,
        ILogger<EntregavelGeradoController> logger)
    {
        _entregavelService = entregavelService;
        _projetoService = projetoService;
        _storageService = storageService;
        _logger = logger;
    }

    [HttpPost("relatorio-projeto")]
    public async Task<IActionResult> GerarRelatorioProjeto(
        Guid projetoId,
        [FromQuery] string formato = "pdf")
    {
        try
        {
            var projeto = await _projetoService.ObterProjetoCompleto(projetoId);
            var htmlContent = GerarHtmlRelatorioProjeto(projeto);

            var (fileContent, fileName, contentType) = await GerarDocumento(htmlContent, formato);
            var filePath = await SalvarDocumento(fileContent, fileName, "relatorios-projeto");

            return Ok(new
            {
                NomeArquivo = fileName,
                UrlDownload = $"/storage/relatorios-projeto/{fileName}",
                CaminhoLocal = filePath
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao gerar relatório de projeto");
            return StatusCode(500, "Erro ao gerar relatório");
        }
    }

    [HttpPost("validacoes-tecnicas")]
    public async Task<IActionResult> GerarDocumentoValidacoes(
        Guid projetoId,
        [FromQuery] string formato = "pdf")
    {
        try
        {
            var validacoes = await _projetoService.ObterValidacoesTecnicas(projetoId);
            var htmlContent = GerarHtmlValidacoesTecnicas(validacoes);

            var (fileContent, fileName, contentType) = await GerarDocumento(htmlContent, formato);
            var filePath = await SalvarDocumento(fileContent, fileName, "validacoes-tecnicas");

            return Ok(new
            {
                NomeArquivo = fileName,
                UrlDownload = $"/storage/validacoes-tecnicas/{fileName}",
                CaminhoLocal = filePath
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao gerar documento de validações técnicas");
            return StatusCode(500, "Erro ao gerar documento");
        }
    }

    [HttpPost("checklist-entregas")]
    public async Task<IActionResult> GerarChecklistEntregas(
        Guid projetoId,
        [FromQuery] string formato = "html")
    {
        try
        {
            var entregas = await _projetoService.ObterEntregasFinais(projetoId);
            var htmlContent = GerarHtmlChecklistEntregas(entregas);

            var (fileContent, fileName, contentType) = await GerarDocumento(htmlContent, formato);
            var filePath = await SalvarDocumento(fileContent, fileName, "checklist-entregas");

            return Ok(new
            {
                NomeArquivo = fileName,
                UrlDownload = $"/storage/checklist-entregas/{fileName}",
                CaminhoLocal = filePath
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao gerar checklist de entregas");
            return StatusCode(500, "Erro ao gerar checklist");
        }
    }

    [HttpPost("kanban-tarefas")]
    public async Task<IActionResult> GerarRelatorioKanban(
        Guid projetoId,
        [FromQuery] string formato = "pdf")
    {
        try
        {
            var tarefas = await _projetoService.ObterTarefasKanban(projetoId);
            var htmlContent = GerarHtmlKanbanTarefas(tarefas);

            var (fileContent, fileName, contentType) = await GerarDocumento(htmlContent, formato);
            var filePath = await SalvarDocumento(fileContent, fileName, "kanban-tarefas");

            return Ok(new
            {
                NomeArquivo = fileName,
                UrlDownload = $"/storage/kanban-tarefas/{fileName}",
                CaminhoLocal = filePath
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao gerar relatório kanban");
            return StatusCode(500, "Erro ao gerar relatório");
        }
    }

    [HttpPost("solucao-proposta")]
    public async Task<IActionResult> GerarSolucaoProposta(
        Guid projetoId,
        [FromQuery] string formato = "pdf")
    {
        try
        {
            var solucao = await _projetoService.ObterSolucaoProposta(projetoId);
            var htmlContent = GerarHtmlSolucaoProposta(solucao);

            var (fileContent, fileName, contentType) = await GerarDocumento(htmlContent, formato);
            var filePath = await SalvarDocumento(fileContent, fileName, "solucao-proposta");

            return Ok(new
            {
                NomeArquivo = fileName,
                UrlDownload = $"/storage/solucao-proposta/{fileName}",
                CaminhoLocal = filePath
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao gerar documento de solução proposta");
            return StatusCode(500, "Erro ao gerar documento");
        }
    }

    private async Task<(byte[] fileContent, string fileName, string contentType)> GerarDocumento(
        string htmlContent, string formato)
    {
        return formato.ToLower() switch
        {
            "pdf" => (
                await _entregavelService.GeneratePdfFromHtmlAsync(htmlContent),
                $"documento_{DateTime.UtcNow:yyyyMMddHHmmss}.pdf",
                "application/pdf"
            ),
            "docx" => (
                await _entregavelService.GenerateDocxFromHtmlAsync(htmlContent),
                $"documento_{DateTime.UtcNow:yyyyMMddHHmmss}.docx",
                "application/vnd.openxmlformats-officedocument.wordprocessingml.document"
            ),
            "html" => (
                Encoding.UTF8.GetBytes(htmlContent),
                $"documento_{DateTime.UtcNow:yyyyMMddHHmmss}.html",
                "text/html"
            ),
            "md" => (
                Encoding.UTF8.GetBytes(await _entregavelService.GenerateMarkdownFromHtmlAsync(htmlContent)),
                $"documento_{DateTime.UtcNow:yyyyMMddHHmmss}.md",
                "text/markdown"
            ),
            _ => throw new ArgumentException("Formato não suportado")
        };
    }

    private async Task<string> SalvarDocumento(byte[] content, string fileName, string container)
    {
        using var stream = new MemoryStream(content);
        var formFile = new FormFile(stream, 0, content.Length, fileName, fileName)
        {
            Headers = new HeaderDictionary(),
            ContentType = "application/octet-stream"
        };

        return await _storageService.UploadFileAsync(formFile, container, fileName);
    }

    private string GerarHtmlRelatorioProjeto(ProjetoConsultoria projeto)
    {
        var sb = new StringBuilder();

        sb.Append($@"
            <!DOCTYPE html>
            <html>
            <head>
                <title>Relatório do Projeto {projeto.Nome}</title>
                <style>
                    body {{ font-family: Arial, sans-serif; margin: 20px; }}
                    h1 {{ color: #2c3e50; }}
                    table {{ border-collapse: collapse; width: 100%; }}
                    th, td {{ border: 1px solid #ddd; padding: 8px; text-align: left; }}
                    th {{ background-color: #f2f2f2; }}
                </style>
            </head>
            <body>
                <h1>Relatório do Projeto: {projeto.Nome}</h1>
                <p><strong>Cliente:</strong> {projeto.Cliente}</p>
                <p><strong>Período:</strong> {projeto.DataInicio} a {projeto.DataEntrega}</p>
    
                <h2>Dados do Projeto</h2>
                <table>
                    <tr><th>Propósito</th><td>{projeto.Proposito}</td></tr>
                    <tr><th>Situação Atual</th><td>{projeto.SituacaoAtual}</td></tr>
                    <tr><th>Status</th><td>{projeto.Status}</td></tr>
                </table>
    
                <h2>Tarefas</h2>
                <table>
                    <tr>
                        <th>Título</th>
                        <th>Status</th>
                        <th>Data Entrega</th>
                    </tr>");

                    foreach (var tarefa in projeto.Tarefas)
                    {
                        sb.Append($@"
                    <tr>
                        <td>{tarefa.Titulo}</td>
                        <td>{tarefa.Status}</td>
                        <td>{tarefa.DataEntrega:dd/MM/yyyy}</td>
                    </tr>");
                    }

                    sb.Append(@"
                </table>
            </body>
            </html>");

        return sb.ToString();
    }
    private string GerarHtmlValidacoesTecnicas(IEnumerable<ValidacaoTecnica> validacoes)
    {
        var sb = new StringBuilder();

        sb.Append(@"
            <!DOCTYPE html>
            <html>
            <head>
                <title>Validações Técnicas</title>
                <style>
                    body { font-family: Arial, sans-serif; margin: 20px; }
                    h1 { color: #2c3e50; }
                    table { border-collapse: collapse; width: 100%; margin-bottom: 20px; }
                    th, td { border: 1px solid #ddd; padding: 8px; text-align: left; }
                    th { background-color: #f2f2f2; }
                    .validado { color: green; }
                    .nao-validado { color: red; }
                </style>
            </head>
            <body>
                <h1>Validações Técnicas</h1>
                <table>
                    <tr>
                        <th>Tipo</th>
                        <th>Descrição</th>
                        <th>URL</th>
                        <th>Status</th>
                        <th>Observações</th>
                    </tr>");

                    foreach (var validacao in validacoes)
                    {
                        sb.Append($@"
                    <tr>
                        <td>{validacao.Tipo}</td>
                        <td>{validacao.Descricao}</td>
                        <td><a href='{validacao.Url}' target='_blank'>Link</a></td>
                        <td class='{(validacao.Validado ? "validado" : "nao-validado")}'>
                            {(validacao.Validado ? "Validado" : "Não Validado")}
                        </td>
                        <td>{validacao.Observacao ?? "-"}</td>
                    </tr>");
                    }

                    sb.Append(@"
                </table>
            </body>
            </html>");

        return sb.ToString();
    }

    private string GerarHtmlChecklistEntregas(IEnumerable<EntregaFinal> entregas)
    {
        // Definir o mapeamento de tipos de entrega
        var tipoEntregaMap = new[]
        {
        new { id = (TipoEntrega)0, label = "Markdown" },
        new { id = (TipoEntrega)1, label = "PDF" },
        new { id = (TipoEntrega)3, label = "Link" }
    };

        var sb = new StringBuilder();

        sb.Append(@"
        <!DOCTYPE html>
        <html>
        <head>
            <title>Checklist de Entregas</title>
            <style>
                body { font-family: Arial, sans-serif; margin: 20px; }
                h1 { color: #2c3e50; }
                table { border-collapse: collapse; width: 100%; margin-bottom: 20px; }
                th, td { border: 1px solid #ddd; padding: 8px; text-align: left; }
                th { background-color: #f2f2f2; }
                .tipo-markdown { background-color: #f8f9fa; }
                .tipo-pdf { background-color: #fff8f8; }
                .tipo-link { background-color: #f8f8ff; }
            </style>
        </head>
        <body>
            <h1>Checklist de Entregas</h1>
            <table>
                <tr>
                    <th>Título</th>
                    <th>Descrição</th>
                    <th>Tipo</th>
                    <th>URL</th>
                    <th>Data Criação</th>
                </tr>");

        foreach (var entrega in entregas)
        {
            var tipoClasse = entrega.Tipo switch
            {
                TipoEntrega.Markdown => "tipo-markdown",
                TipoEntrega.Pdf => "tipo-pdf",
                TipoEntrega.Link => "tipo-link",
                _ => ""
            };

            sb.Append($@"
                <tr class='{tipoClasse}'>
                    <td>{entrega.Titulo}</td>
                    <td>{entrega.Descricao}</td>
                    <td>{tipoEntregaMap.FirstOrDefault(t => t.id == entrega.Tipo)?.label ?? "Outro"}</td>
                    <td><a href='{entrega.UrlEntrega}' target='_blank'>Acessar</a></td>
                    <td>{entrega.CriadoEm:dd/MM/yyyy HH:mm}</td>
                </tr>");
        }

        sb.Append(@"
            </table>
        </body>
        </html>");

        return sb.ToString();
    }

    private string GerarHtmlKanbanTarefas(IEnumerable<TarefaProjeto> tarefas)
    {
        // Definir o mapeamento de status
        var statusMap = new[]
        {
        new { id = (StatusTarefa)0, title = "A Fazer", color = "#e74c3c" },
        new { id = (StatusTarefa)1, title = "Em Progresso", color = "#f39c12" },
        new { id = (StatusTarefa)2, title = "Em Revisão", color = "#3498db" },
        new { id = (StatusTarefa)3, title = "Concluído", color = "#2ecc71" }
    };

        var sb = new StringBuilder();

        sb.Append(@"
        <!DOCTYPE html>
        <html>
        <head>
            <title>Kanban de Tarefas</title>
            <style>
                body { font-family: Arial, sans-serif; margin: 20px; }
                h1 { color: #2c3e50; }
                .kanban-container { display: flex; gap: 15px; }
                .kanban-column { flex: 1; border: 1px solid #ddd; border-radius: 5px; padding: 10px; }
                .kanban-title { font-weight: bold; text-align: center; padding: 5px; margin-bottom: 10px; }
                .task-card { background: white; border: 1px solid #eee; border-radius: 3px; padding: 8px; margin-bottom: 8px; }
                .task-title { font-weight: bold; }
                .task-desc { color: #555; font-size: 0.9em; }
                .task-date { color: #777; font-size: 0.8em; text-align: right; }
            </style>
        </head>
        <body>
            <h1>Kanban de Tarefas</h1>
            <div class='kanban-container'>");

        // Agrupar tarefas por status
        var tarefasPorStatus = tarefas.GroupBy(t => t.Status);

        foreach (var grupo in tarefasPorStatus)
        {
            var statusInfo = statusMap.FirstOrDefault(s => s.id == grupo.Key) ?? statusMap[0];

            sb.Append($@"
                <div class='kanban-column'>
                    <div class='kanban-title' style='color: {statusInfo.color}'>{statusInfo.title}</div>");

            foreach (var tarefa in grupo)
            {
                sb.Append($@"
                    <div class='task-card'>
                        <div class='task-title'>{tarefa.Titulo}</div>
                        <div class='task-desc'>{tarefa.Descricao}</div>
                        <div class='task-date'>Entrega: {tarefa.DataEntrega:dd/MM/yyyy}</div>
                    </div>");
            }

            sb.Append(@"
                </div>");
        }

        sb.Append(@"
            </div>
        </body>
        </html>");

        return sb.ToString();
    }

    private string GerarHtmlSolucaoProposta(SolucaoProposta solucao)
    {
        var sb = new StringBuilder();

        sb.Append($@"
            <!DOCTYPE html>
            <html>
            <head>
                <title>Solução Proposta</title>
                <style>
                    body {{ font-family: Arial, sans-serif; margin: 20px; line-height: 1.6; }}
                    h1 {{ color: #2c3e50; border-bottom: 2px solid #eee; padding-bottom: 10px; }}
                    h2 {{ color: #3a5169; margin-top: 20px; }}
                    .section {{ margin-bottom: 20px; }}
                    .riscos {{ background-color: #fff8f8; padding: 15px; border-left: 4px solid #ff6b6b; }}
                    .recomendacoes {{ background-color: #f8fff8; padding: 15px; border-left: 4px solid #81c784; }}
                </style>
            </head>
            <body>
                <h1>Solução Proposta</h1>
    
                <div class='section'>
                    <h2>Resumo</h2>
                    <p>{solucao.Resumo}</p>
                </div>
    
                <div class='section'>
                    <h2>Arquitetura</h2>
                    <p>{solucao.Arquitetura}</p>
                </div>
    
                <div class='section'>
                    <h2>Componentes do Sistema</h2>
                    <p>{solucao.ComponentesSistema}</p>
                </div>
    
                <div class='section'>
                    <h2>Pontos de Integração</h2>
                    <p>{solucao.PontosIntegracao}</p>
                </div>
    
                <div class='section riscos'>
                    <h2>Riscos</h2>
                    <p>{solucao.Riscos}</p>
                </div>
    
                <div class='section recomendacoes'>
                    <h2>Recomendações Técnicas</h2>
                    <p>{solucao.RecomendacoesTecnicas}</p>
                </div>
    
                <div class='section'>
                    <p><small>Documento gerado em: {DateTime.Now:dd/MM/yyyy HH:mm}</small></p>
                </div>
            </body>
            </html>");

        return sb.ToString();
    }

    private static readonly List<(int id, string label)> tipoEntregaMap = new()
    {
        (0, "Markdown"),
        (1, "PDF"),
        (2, "DOC"),
        (3, "Link"),
        (4, "ZIP"),
        (5, "Outro")
    };

    private static readonly List<(int id, string title, string color)> statusMap = new()
    {
        (0, "Pendente", "#FFEE58"),
        (1, "Em Andamento", "#4FC3F7"),
        (2, "Em Impedimento", "#FF8A65"),
        (3, "Em Pausa", "#BA68C8"),
        (4, "Feito", "#81C784")
    };
}