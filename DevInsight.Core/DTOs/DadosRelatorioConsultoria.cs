using DevInsight.Core.Entities;

namespace DevInsight.Core.DTOs;

public class DadosRelatorioConsultoria
{
    // Dados básicos do projeto
    public string NomeProjeto { get; set; }
    public string Cliente { get; set; }
    public string Consultor { get; set; }
    public DateOnly DataEntrega { get; set; }
    public string Proposito { get; set; }
    public string SituacaoAtual { get; set; }

    // Seção 1 - Diagnóstico e Levantamento
    public List<Requisito> RequisitosFuncionais { get; set; } = new List<Requisito>();
    public List<Requisito> RequisitosNaoFuncionais { get; set; } = new List<Requisito>();
    public List<PersonasChave> PersonasChaves { get; set; } = new List<PersonasChave>();
    public List<StakeHolder> StakeHolders { get; set; } = new List<StakeHolder>();
    public List<FuncionalidadeDesejada> Funcionalidades {  get; set; } = new List<FuncionalidadeDesejada>();
    public List<EntregavelGerado> Documentos {  get; set; } = new List<EntregavelGerado>();

    // Seção 2 - Proposta de Solução
    public SolucaoProposta SolucaoProposta { get; set; }
    public DocumentoLink DiagramaSolucao { get; set; }
    public DocumentoLink PrototipoTelas { get; set; }

    // Seção 3 - Planejamento e Roadmap
    public List<FaseProjeto> FasesProjeto { get; set; } = new List<FaseProjeto>();
    public List<EstimativaCusto> EstimativasCusto { get; set; } = new List<EstimativaCusto>();
    public decimal TotalEstimado { get; set; }

    // Seção 4 - Validações Técnicas
    public List<ValidacaoTecnica> ValidacoesTecnicas { get; set; } = new List<ValidacaoTecnica>();

    // Seção 5 - Documentação Técnica
    public List<EntregaFinal> EntregasFinais { get; set; } = new List<EntregaFinal>();
    public List<EntregavelGerado> EntregaveisGerados { get; set; } = new List<EntregavelGerado>();

    // Seção 6 - Checklist Final
    public List<TarefaProjeto> Tarefas { get; set; } = new List<TarefaProjeto>();

    // Seção 7 - Plano de Ação Pós-Consultoria
    public string RecomendacoesTecnicas { get; set; }

    // Seção 8 - Contato
    public string EmailConsultor { get; set; }
}