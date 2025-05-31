namespace DevInsight.Core.Entities;

public class EstimativaCusto
{
    public Guid Id { get; set; }
    public Guid ProjetoId { get; set; }
    public ProjetoConsultoria Projeto { get; set; } = null!;
    public string Item {  get; set; } // Levantamento, Desenvolvimento MVP, QA/Testes, etc...
    public int EstimativaHoras { get; set; }
    public double ValorHoras { get; set; }
    public double CustoEstimado { get; set; }
    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;
}
