using DevInsight.Core.Enums;

namespace DevInsight.Core.DTOs;

public class EntregaFinalCriacaoDTO
{
    public string Titulo { get; set; } = null!;
    public string Descricao { get; set; } = null!;
    public string UrlEntrega { get; set; } = null!;
    public TipoEntrega Tipo { get; set; }
}

public class EntregaFinalAtualizacaoDTO
{
    public string Titulo { get; set; } = null!;
    public string Descricao { get; set; } = null!;
    public string UrlEntrega { get; set; } = null!;
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