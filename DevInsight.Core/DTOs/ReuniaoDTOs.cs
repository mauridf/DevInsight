using DevInsight.Core.Entities;

namespace DevInsight.Core.DTOs;

public class ReuniaoCriacaoDTO
{
    public Guid ProjetoId { get; set; }
    public string Titulo { get; set; } = null!;
    public DateTime DataHora { get; set; }
    public string Link { get; set; } = null!;
    public string? Observacoes { get; set; }
}

public class ReuniaoAtualizacaoDTO
{
    public Guid ProjetoId { get; set; }
    public string Titulo { get; set; } = null!;
    public DateTime DataHora { get; set; }
    public string Link { get; set; } = null!;
    public string? Observacoes { get; set; }
}

public class ReuniaoConsultaDTO
{
    public Guid Id { get; set; }
    public Guid ProjetoId { get; set; }
    public ProjetoConsultoria Projeto { get; set; }
    public string Titulo { get; set; } = null!;
    public DateTime DataHora { get; set; }
    public string Link { get; set; } = null!;
    public string? Observacoes { get; set; }
    public DateTime CriadoEm { get; set; }
}