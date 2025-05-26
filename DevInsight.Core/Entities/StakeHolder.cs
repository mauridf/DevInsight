namespace DevInsight.Core.Entities;

public class StakeHolder
{
    public Guid Id { get; set; }
    public Guid ProjetoId { get; set; }
    public ProjetoConsultoria Projeto { get; set; } = null!;
    public string Nome { get; set; } = null!;
    public string Funcao { get; set; } = null!;
    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;
}
