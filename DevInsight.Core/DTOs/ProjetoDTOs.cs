using DevInsight.Core.Enums;

namespace DevInsight.Core.DTOs;

public class ProjetoCriacaoDTO
{
    public string Nome { get; set; } = null!;
    public string Cliente { get; set; } = null!;
    public Guid CriadoPorId { get; set; }
    public DateOnly DataInicio { get; set; }
    public DateOnly DataEntrega { get; set; }
    public string Proposito { get; set; }
    public string SituacaoAtual { get; set; }
    public StatusProjeto Status { get; set; }
}

public class ProjetoAtualizacaoDTO
{
    public string Nome { get; set; } = null!;
    public string Cliente { get; set; } = null!;
    public Guid CriadoPorId { get; set; }
    public DateOnly DataInicio { get; set; }
    public DateOnly DataEntrega { get; set; }
    public string Proposito { get; set; }
    public string SituacaoAtual { get; set; }
    public StatusProjeto Status { get; set; }
}

public class ProjetoConsultaDTO
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = null!;
    public string Cliente { get; set; } = null!;
    public Guid CriadoPorId { get; set; }
    public string CriadoPorNome { get; set; } = null!;
    public DateOnly DataInicio { get; set; }
    public DateOnly DataEntrega { get; set; }
    public string Proposito { get; set; }
    public string SituacaoAtual { get; set; }
    public StatusProjeto Status { get; set; }
    public DateTime CriadoEm { get; set; }
}

public class ProjetoDetalhesDTO
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = null!;
    public string Cliente { get; set; } = null!;
    public UsuarioConsultaDTO CriadoPor { get; set; } = null!;
    public DateOnly DataInicio { get; set; }
    public DateOnly DataEntrega { get; set; }
    public string Proposito { get; set; }
    public string SituacaoAtual { get; set; }
    public StatusProjeto Status { get; set; }
    public DateTime CriadoEm { get; set; }
    public int TotalStakeholders { get; set; }
    public int TotalRequisitos { get; set; }
    public int TotalTarefas { get; set; }
}
