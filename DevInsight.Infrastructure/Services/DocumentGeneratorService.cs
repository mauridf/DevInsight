using DevInsight.Core.Enums;
using Microsoft.Extensions.Logging;

namespace DevInsight.Infrastructure.Services;

public interface IDocumentGeneratorService
{
    Task<byte[]> GeneratePdfAsync(string content, FormatoEntregavel formato);
    Task<string> GenerateMarkdownAsync(string content);
}

public class DocumentGeneratorService : IDocumentGeneratorService
{
    private readonly ILogger<DocumentGeneratorService> _logger;

    public DocumentGeneratorService(ILogger<DocumentGeneratorService> logger)
    {
        _logger = logger;
    }

    public async Task<byte[]> GeneratePdfAsync(string content, FormatoEntregavel formato)
    {
        try
        {
            // Implementação usando uma biblioteca como QuestPDF ou iTextSharp
            // Exemplo simplificado:
            using var memoryStream = new MemoryStream();
            // Lógica de geração de PDF aqui
            return memoryStream.ToArray();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao gerar PDF");
            throw;
        }
    }

    public async Task<string> GenerateMarkdownAsync(string content)
    {
        // Lógica simples de formatação Markdown
        return $"# Documento Gerado\n\n{content}";
    }
}