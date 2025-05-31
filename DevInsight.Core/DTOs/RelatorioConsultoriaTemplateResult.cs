namespace DevInsight.Core.DTOs;

public class RelatorioConsultoriaTemplateResult
{
    /// <summary>
    /// Conteúdo do relatório formatado em HTML
    /// </summary>
    public string HtmlContent { get; set; }

    /// <summary>
    /// Conteúdo do relatório em texto puro (formato markdown-like)
    /// </summary>
    public string RawContent { get; set; }
}
