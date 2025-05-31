using DevInsight.Core.Entities;
using Microsoft.AspNetCore.Http;

namespace DevInsight.Core.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IRepository<Usuario> Usuarios { get; }
    IRepository<ProjetoConsultoria> Projetos { get; }
    IRepository<PersonasChave> PersonasChaves { get; }
    IRepository<FaseProjeto> FasesProjeto { get; }
    IRepository<EstimativaCusto> EstimativasCustos { get; }
    IRepository<StakeHolder> StakeHolders { get; }
    IRepository<FuncionalidadeDesejada> Funcionalidades { get; }
    IRepository<Requisito> Requisitos { get; }
    IRepository<DocumentoLink> Documentos { get; }
    IRepository<Reuniao> Reunioes { get; }
    IRepository<TarefaProjeto> Tarefas { get; }
    IRepository<ValidacaoTecnica> ValidacoesTecnicas { get; }
    IRepository<EntregaFinal> Entregas { get; }
    IRepository<SolucaoProposta> Solucoes { get; }
    IRepository<EntregavelGerado> Entregaveis { get; }
    Task<int> CompleteAsync();
}
