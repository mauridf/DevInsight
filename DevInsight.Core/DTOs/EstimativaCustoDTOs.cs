using DevInsight.Core.Entities;

namespace DevInsight.Core.DTOs;

public class EstimativaCustoCriacaoDTO
{
    public Guid ProjetoId { get; set; }
    public ProjetoConsultoria Projeto { get; set; } = null!;
    public string Item { get; set; }
    public int EstimativaHoras { get; set; }
    public double ValorHoras { get; set; }
    public double CustoEstimado { get; set; }
}

public class EstimativaCustoAtualizacaoDTO
{
    public Guid ProjetoId { get; set; }
    public ProjetoConsultoria Projeto { get; set; } = null!;
    public string Item { get; set; }
    public int EstimativaHoras { get; set; }
    public double ValorHoras { get; set; }
    public double CustoEstimado { get; set; }
}

public class EstimativaCustoConsultaDTO
{
    public Guid Id { get; set; }
    public Guid ProjetoId { get; set; }
    public ProjetoConsultoria Projeto { get; set; } = null!;
    public string Item { get; set; }
    public int EstimativaHoras { get; set; }
    public double ValorHoras { get; set; }
    public double CustoEstimado { get; set; }
    public DateTime CriadoEm { get; set; }
}


