using System.ComponentModel.DataAnnotations;
using DevInsight.Core.Enums;

namespace DevInsight.Core.DTOs;

public class DocumentoLinkCriacaoDTO
{
    public TipoDocumento TipoDocumento { get; set; }
    [Required]
    [MaxLength(500)]
    public string Descricao { get; set; } = null!;
    [Required]
    [Url]
    [MaxLength(500)]
    public string Url { get; set; } = null!;
}

public class DocumentoLinkAtualizacaoDTO
{
    public TipoDocumento TipoDocumento { get; set; }
    public string Descricao { get; set; } = null!;
    public string Url { get; set; } = null!;
}

public class DocumentoLinkConsultaDTO
{
    public Guid Id { get; set; }
    public TipoDocumento TipoDocumento { get; set; }
    public string Descricao { get; set; } = null!;
    public string Url { get; set; } = null!;
    public DateTime CriadoEm { get; set; }
}