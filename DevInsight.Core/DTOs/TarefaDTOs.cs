using System.ComponentModel.DataAnnotations;
using DevInsight.Core.Attributes;
using DevInsight.Core.Entities;
using DevInsight.Core.Enums;

namespace DevInsight.Core.DTOs;

public class TarefaCriacaoDTO
{
    public Guid ProjetoId { get; set; }
    [Required]
    [MaxLength(200)]
    public string Titulo { get; set; } = null!;
    [Required]
    public string Descricao { get; set; } = null!;
    [Required]
    public StatusTarefa Status { get; set; }
    [Required]
    [FutureDate(ErrorMessage = "Data de entrega deve ser no futuro")]
    public DateTime DataEntrega { get; set; }
}

public class TarefaAtualizacaoDTO
{
    public Guid ProjetoId { get; set; }
    [Required]
    [MaxLength(200)]
    public string Titulo { get; set; } = null!;
    [Required]
    public string Descricao { get; set; } = null!;
    [Required]
    public StatusTarefa Status { get; set; }
    [Required]
    [FutureDate(ErrorMessage = "Data de entrega deve ser no futuro")]
    public DateTime DataEntrega { get; set; }
}

public class TarefaConsultaDTO
{
    public Guid Id { get; set; }
    public Guid ProjetoId { get; set; }
    public ProjetoConsultoria Projeto { get; set; }
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