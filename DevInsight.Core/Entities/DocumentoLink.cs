using DevInsight.Core.Enums;

namespace DevInsight.Core.Entities;

public class DocumentoLink
{
    public Guid Id { get; set; }
    public Guid ProjetoId { get; set; }
    public ProjetoConsultoria Projeto { get; set; } = null!;
    public TipoDocumento TipoDocumento { get; set; }
    public string Descricao { get; set; }
    public string Url { get; set; }
    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;
}
