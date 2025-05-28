using DevInsight.Core.Enums;

namespace DevInsight.Core.DTOs;

public class EntregavelGeradoCriacaoDTO
{
    public TipoEntregavel Tipo { get; set; }
    public FormatoEntregavel Formato { get; set; }
    public string NomeArquivo { get; set; }
    public string Conteudo { get; set; } = null!; // Para markdown ou HTML
    public byte[]? Arquivo { get; set; } // Para PDF
    public string UrlDownload { get; set; }
}

public class EntregavelGeradoConsultaDTO
{
    public Guid Id { get; set; }
    public TipoEntregavel Tipo { get; set; }
    public FormatoEntregavel Formato { get; set; }
    public string NomeArquivo { get; set; }
    public string Conteudo { get; set; } = null!; // Para markdown ou HTML
    public byte[]? Arquivo { get; set; } // Para PDF
    public string UrlDownload { get; set; }
    public DateTime DataGeracao { get; set; }
}

public class GerarDocumentoDTO
{
    public TipoEntregavel Tipo { get; set; }
    public FormatoEntregavel Formato { get; set; }
    public Guid ProjetoId { get; set; }
    public string NomeArquivo { get; set; }
    public string Conteudo { get; set; } = null!;
    public byte[]? Arquivo { get; set; } // Para PDF
    public string UrlDownload { get; set; }
}