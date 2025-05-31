using DevInsight.Core.Entities;
using DevInsight.Core.Enums;

namespace DevInsight.Core.DTOs;

public class PersonaChaveCriacaoDTO
{
    public Guid ProjetoId { get; set; }
    public string Persona { get; set; }
    public Perfil Perfil { get; set; }
    public TipoPerfil Tipo { get; set; }
    public string Necessidade { get; set; }
}

public class PersonaChaveAtualizacaoDTO
{
    public Guid ProjetoId { get; set; }
    public string Persona { get; set; }
    public Perfil Perfil { get; set; }
    public TipoPerfil Tipo { get; set; }
    public string Necessidade { get; set; }
}

public class PersonaChaveConsultaDTO
{
    public Guid Id { get; set; }
    public Guid ProjetoId { get; set; }
    public ProjetoConsultoria Projeto { get; set; } = null!;
    public string Persona { get; set; }
    public Perfil Perfil { get; set; }
    public TipoPerfil Tipo { get; set; }
    public string Necessidade { get; set; }
    public DateTime CriadoEm { get; set; }
}

