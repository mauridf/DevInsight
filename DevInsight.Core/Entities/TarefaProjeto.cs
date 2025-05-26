using DevInsight.Core.Enums;

namespace DevInsight.Core.Entities;

public class TarefaProjeto
{
    public Guid Id { get; set; }
    public Guid ProjetoId { get; set; }
    public ProjetoConsultoria Projeto { get; set; } = null!;
    public string Titulo { get; set; }
    public string Descricao { get; set; }
    public StatusTarefa Status { get; set; }
    public DateTime DataEntrega { get; set; }
    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;
}
