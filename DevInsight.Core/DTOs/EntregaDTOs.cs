using System.ComponentModel.DataAnnotations;
using DevInsight.Core.Entities;
using DevInsight.Core.Enums;

namespace DevInsight.Core.DTOs;

public class EntregaFinalCriacaoDTO
{
    public Guid Id { get; set; }
    public Guid ProjetoId { get; set; }
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
    public Guid Id { get; set; }
    public Guid ProjetoId { get; set; }
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
    public Guid ProjetoId { get; set; }
    public ProjetoConsultoria Projeto { get; set; }
    public string Titulo { get; set; } = null!;
    public string Descricao { get; set; } = null!;
    public string UrlEntrega { get; set; } = null!;
    public TipoEntrega Tipo { get; set; }
    public DateTime CriadoEm { get; set; }
}