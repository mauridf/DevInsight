using DevInsight.Core.Enums;

namespace DevInsight.Core.Entities;

public class Requisito
{
    public Guid Id { get; set; }
    public Guid ProjetoId { get; set; }
    public ProjetoConsultoria Projeto { get; set; } = null!;
    public TipoRequisito TipoRequisito { get; set; }
    public string Descricao { get; set; }
    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;
}
