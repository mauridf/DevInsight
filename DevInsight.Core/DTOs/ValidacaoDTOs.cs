using System.ComponentModel.DataAnnotations;
using DevInsight.Core.Enums;

namespace DevInsight.Core.DTOs;

public class ValidacaoTecnicaCriacaoDTO
{
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
    public TipoValidacao Tipo { get; set; }
    public string Descricao { get; set; } = null!;
    public string Url { get; set; } = null!;
    public bool Validado { get; set; }
    public string? Observacao { get; set; }
    public DateTime CriadoEm { get; set; }
}