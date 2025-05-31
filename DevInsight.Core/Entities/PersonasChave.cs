using DevInsight.Core.Enums;

namespace DevInsight.Core.Entities;

public class PersonasChave
{
    public Guid Id { get; set; }
    public Guid ProjetoId { get; set; }
    public ProjetoConsultoria Projeto { get; set; } = null!;
    public string Persona { get; set; }
    public Perfil Perfil { get; set; }
    public TipoPerfil Tipo {  get; set; }
    public string Necessidade { get; set; }
    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;
}
