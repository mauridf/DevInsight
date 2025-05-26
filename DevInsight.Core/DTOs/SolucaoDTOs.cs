namespace DevInsight.Core.DTOs;

public class SolucaoPropostaCriacaoDTO
{
    public string Resumo { get; set; } = null!;
    public string Arquitetura { get; set; } = null!;
    public string ComponentesSistema { get; set; } = null!;
    public string PontosIntegracao { get; set; } = null!;
    public string Riscos { get; set; } = null!;
    public string RecomendacoesTecnicas { get; set; } = null!;
}

public class SolucaoPropostaAtualizacaoDTO
{
    public string Resumo { get; set; } = null!;
    public string Arquitetura { get; set; } = null!;
    public string ComponentesSistema { get; set; } = null!;
    public string PontosIntegracao { get; set; } = null!;
    public string Riscos { get; set; } = null!;
    public string RecomendacoesTecnicas { get; set; } = null!;
}

public class SolucaoPropostaConsultaDTO
{
    public Guid Id { get; set; }
    public string Resumo { get; set; } = null!;
    public string Arquitetura { get; set; } = null!;
    public string ComponentesSistema { get; set; } = null!;
    public string PontosIntegracao { get; set; } = null!;
    public string Riscos { get; set; } = null!;
    public string RecomendacoesTecnicas { get; set; } = null!;
    public DateTime CriadoEm { get; set; }
}