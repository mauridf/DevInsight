namespace DevInsight.Core.Entities;

public class FaseProjeto
{
    public Guid Id { get; set; }
    public Guid ProjetoId { get; set; }
    public ProjetoConsultoria Projeto { get; set; } = null!;
    public string Fase {  get; set; }
    public string Objetivo { get; set; }
    public int DuracaoEstimada { get; set; } // Em Semanas (Ex.: 1, 2, 3, etc...)
    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;
}
