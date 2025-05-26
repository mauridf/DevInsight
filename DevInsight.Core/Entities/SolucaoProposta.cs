using DevInsight.Core.Enums;

namespace DevInsight.Core.Entities;

public class SolucaoProposta
{
    public Guid Id { get; set; }
    public Guid ProjetoId { get; set; }
    public ProjetoConsultoria Projeto { get; set; } = null!;
    public string Resumo { get; set; }
    public string Arquitetura { get; set; }
    public string ComponentesSistema { get; set; }
    public string PontosIntegracao { get; set; }
    public string Riscos { get; set; }
    public string RecomendacoesTecnicas { get; set; }
    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;
}
