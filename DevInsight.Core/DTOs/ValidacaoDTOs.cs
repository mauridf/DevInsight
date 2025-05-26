using DevInsight.Core.Enums;

namespace DevInsight.Core.DTOs;

public class ValidacaoTecnicaCriacaoDTO
{
    public TipoValidacao Tipo { get; set; }
    public string Descricao { get; set; } = null!;
    public string Url { get; set; } = null!;
    public string? Observacao { get; set; }
}

public class ValidacaoTecnicaAtualizacaoDTO
{
    public TipoValidacao Tipo { get; set; }
    public string Descricao { get; set; } = null!;
    public string Url { get; set; } = null!;
    public bool Validado { get; set; }
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