using System.ComponentModel.DataAnnotations;
using DevInsight.Core.Entities;
using DevInsight.Core.Enums;

namespace DevInsight.Core.DTOs;

public class ValidacaoTecnicaCriacaoDTO
{
    public Guid ProjetoId { get; set; }
    [Required]
    public TipoValidacao Tipo { get; set; }
    [Required]
    [MaxLength(1000)]
    public string Descricao { get; set; } = null!;
    [Required]
    [Url]
    [MaxLength(500)]
    public string Url { get; set; } = null!;
    [MaxLength(2000)]
    public string? Observacao { get; set; }
}

public class ValidacaoTecnicaAtualizacaoDTO
{
    public Guid ProjetoId { get; set; }
    [Required]
    public TipoValidacao Tipo { get; set; }
    [Required]
    [MaxLength(1000)]
    public string Descricao { get; set; } = null!;
    [Required]
    [Url]
    [MaxLength(500)]
    public string Url { get; set; } = null!;
    [MaxLength(2000)]
    public string? Observacao { get; set; }
}

public class ValidacaoTecnicaConsultaDTO
{
    public Guid Id { get; set; }
    public Guid ProjetoId { get; set; }
    public ProjetoConsultoria Projeto { get; set; }
    public TipoValidacao Tipo { get; set; }
    public string Descricao { get; set; } = null!;
    public string Url { get; set; } = null!;
    public bool Validado { get; set; }
    public string? Observacao { get; set; }
    public DateTime CriadoEm { get; set; }
}