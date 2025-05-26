using DevInsight.Core.Enums;

namespace DevInsight.Core.Entities;

public class EntregaFinal
{
    public Guid Id { get; set; }
    public Guid ProjetoId { get; set; }
    public ProjetoConsultoria Projeto { get; set; } = null!;
    public string Titulo { get; set; }
    public string Descricao { get; set; }
    public string UrlEntrega { get; set; }
    public TipoEntrega Tipo { get; set; }
    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;
}
