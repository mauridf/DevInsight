using DevInsight.Core.Enums;

namespace DevInsight.Core.DTOs;

public class DocumentoLinkCriacaoDTO
{
    public TipoDocumento TipoDocumento { get; set; }
    public string Descricao { get; set; } = null!;
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