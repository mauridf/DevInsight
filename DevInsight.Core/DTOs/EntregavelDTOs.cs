using DevInsight.Core.Enums;

namespace DevInsight.Core.DTOs;

public class EntregavelGeradoCriacaoDTO
{
    public TipoEntregavel Tipo { get; set; }
    public FormatoEntregavel Formato { get; set; }
}

public class EntregavelGeradoConsultaDTO
{
    public Guid Id { get; set; }
    public TipoEntregavel Tipo { get; set; }
    public FormatoEntregavel Formato { get; set; }
    public DateTime DataGeracao { get; set; }
    public string? UrlDownload { get; set; }
}