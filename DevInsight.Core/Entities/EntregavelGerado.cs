using DevInsight.Core.Enums;

namespace DevInsight.Core.Entities;

public class EntregavelGerado
{
    public Guid Id { get; set; }
    public Guid ProjetoId { get; set; }
    public ProjetoConsultoria Projeto { get; set; } = null!;
    public TipoEntregavel Tipo { get; set; }
    public FormatoEntregavel Formato { get; set; }
    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;
}
