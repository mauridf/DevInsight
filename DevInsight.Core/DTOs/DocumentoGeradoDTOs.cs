using DevInsight.Core.Enums;

namespace DevInsight.Core.DTOs;

public class DocumentoGeradoDTO
{
    public TipoEntregavel Tipo { get; set; }
    public FormatoEntregavel Formato { get; set; }
    public string Conteudo { get; set; } = null!; // Para markdown ou HTML
    public byte[]? Arquivo { get; set; } // Para PDF
}

public class GerarDocumentoDTO
{
    public TipoEntregavel Tipo { get; set; }
    public FormatoEntregavel Formato { get; set; }
    public Guid ProjetoId { get; set; }
}