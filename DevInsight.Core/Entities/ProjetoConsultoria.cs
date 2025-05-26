using DevInsight.Core.Enums;

namespace DevInsight.Core.Entities;

public class ProjetoConsultoria
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = null!;
    public string Cliente { get; set; } = null!;
    public Guid CriadoPorId { get; set; }
    public Usuario CriadoPor { get; set; } = null!;
    public DateOnly DataInicio { get; set; }
    public StatusProjeto Status { get; set; }
    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;

    // Relacionamentos
    public ICollection<StakeHolder> StakeHolders { get; set; } = new List<StakeHolder>();
    public ICollection<FuncionalidadeDesejada> Funcionalidades { get; set; } = new List<FuncionalidadeDesejada>();
    public ICollection<Requisito> Requisitos { get; set; } = new List<Requisito>();
    public ICollection<DocumentoLink> Documentos { get; set; } = new List<DocumentoLink>();
    public ICollection<Reuniao> Reunioes { get; set; } = new List<Reuniao>();
    public ICollection<TarefaProjeto> Tarefas { get; set; } = new List<TarefaProjeto>();
    public ICollection<ValidacaoTecnica> ValidacoesTecnicas { get; set; } = new List<ValidacaoTecnica>();
    public ICollection<EntregaFinal> Entregas { get; set; } = new List<EntregaFinal>();
    public ICollection<SolucaoProposta> Solucoes { get; set; } = new List<SolucaoProposta>();
    public ICollection<EntregavelGerado> Entregaveis { get; set; } = new List<EntregavelGerado>();
}