using DevInsight.Core.Enums;

namespace DevInsight.Core.Entities;

public class ValidacaoTecnica
{
    public Guid Id { get; set; }
    public Guid ProjetoId { get; set; }
    public ProjetoConsultoria Projeto { get; set; } = null!;
    public TipoValidacao Tipo { get; set; }
    public string Descricao { get; set; }
    public string Url { get; set; }
    public bool Validado { get; set; }
    public string Observacao { get; set; }
    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;
}
