using DevInsight.Core.Enums;

namespace DevInsight.Core.DTOs;

public class RequisitoCriacaoDTO
{
    public TipoRequisito TipoRequisito { get; set; }
    public string Descricao { get; set; } = null!;
}

public class RequisitoAtualizacaoDTO
{
    public TipoRequisito TipoRequisito { get; set; }
    public string Descricao { get; set; } = null!;
}

public class RequisitoConsultaDTO
{
    public Guid Id { get; set; }
    public TipoRequisito TipoRequisito { get; set; }
    public string Descricao { get; set; } = null!;
    public DateTime CriadoEm { get; set; }
}