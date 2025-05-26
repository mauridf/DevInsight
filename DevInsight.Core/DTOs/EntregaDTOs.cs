using System.ComponentModel.DataAnnotations;
using DevInsight.Core.Enums;

namespace DevInsight.Core.DTOs;

public class EntregaFinalCriacaoDTO
{
    [Required]
    [MaxLength(200)]
    public string Titulo { get; set; } = null!;
    [Required]
    [MaxLength(2000)]
    public string Descricao { get; set; } = null!;
    [Required]
    [Url]
    [MaxLength(500)]
    public string UrlEntrega { get; set; } = null!;
    [Required]
    public TipoEntrega Tipo { get; set; }
}

public class EntregaFinalAtualizacaoDTO
{
    [Required]
    [MaxLength(200)]
    public string Titulo { get; set; } = null!;
    [Required]
    [MaxLength(2000)]
    public string Descricao { get; set; } = null!;
    [Required]
    [Url]
    [MaxLength(500)]
    public string UrlEntrega { get; set; } = null!;
    [Required]
    public TipoEntrega Tipo { get; set; }
}

public class EntregaFinalConsultaDTO
{
    public Guid Id { get; set; }
    public string Titulo { get; set; } = null!;
    public string Descricao { get; set; } = null!;
    public string UrlEntrega { get; set; } = null!;
    public TipoEntrega Tipo { get; set; }
    public DateTime CriadoEm { get; set; }
}