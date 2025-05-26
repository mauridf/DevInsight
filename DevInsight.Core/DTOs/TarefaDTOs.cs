using DevInsight.Core.Enums;

namespace DevInsight.Core.DTOs;

public class TarefaCriacaoDTO
{
    public string Titulo { get; set; } = null!;
    public string Descricao { get; set; } = null!;
    public StatusTarefa Status { get; set; }
    public DateTime DataEntrega { get; set; }
}

public class TarefaAtualizacaoDTO
{
    public string Titulo { get; set; } = null!;
    public string Descricao { get; set; } = null!;
    public StatusTarefa Status { get; set; }
    public DateTime DataEntrega { get; set; }
}

public class TarefaConsultaDTO
{
    public Guid Id { get; set; }
    public string Titulo { get; set; } = null!;
    public string Descricao { get; set; } = null!;
    public StatusTarefa Status { get; set; }
    public DateTime DataEntrega { get; set; }
    public DateTime CriadoEm { get; set; }
}

public class TarefaKanbanDTO
{
    public Guid Id { get; set; }
    public string Titulo { get; set; } = null!;
    public StatusTarefa Status { get; set; }
    public DateTime DataEntrega { get; set; }
}