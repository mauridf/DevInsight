using DevInsight.Core.Entities;
using DevInsight.Core.Interfaces;

namespace DevInsight.Infrastructure.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        Usuarios = new Repository<Usuario>(_context);
        Projetos = new Repository<ProjetoConsultoria>(_context);
        StakeHolders = new Repository<StakeHolder>(_context);
        Funcionalidades = new Repository<FuncionalidadeDesejada>(_context);
        Requisitos = new Repository<Requisito>(_context);
        Documentos = new Repository<DocumentoLink>(_context);
        Reunioes = new Repository<Reuniao>(_context);
        Tarefas = new Repository<TarefaProjeto>(_context);
        ValidacoesTecnicas = new Repository<ValidacaoTecnica>(_context);
        Entregas = new Repository<EntregaFinal>(_context);
        Solucoes = new Repository<SolucaoProposta>(_context);
        Entregaveis = new Repository<EntregavelGerado>(_context);
    }

    public IRepository<Usuario> Usuarios { get; }

    public IRepository<ProjetoConsultoria> Projetos { get; }

    public IRepository<StakeHolder> StakeHolders { get; }

    public IRepository<FuncionalidadeDesejada> Funcionalidades { get; }

    public IRepository<Requisito> Requisitos { get; }

    public IRepository<DocumentoLink> Documentos { get; }

    public IRepository<Reuniao> Reunioes { get; }

    public IRepository<TarefaProjeto> Tarefas { get; }

    public IRepository<ValidacaoTecnica> ValidacoesTecnicas { get; }

    public IRepository<EntregaFinal> Entregas { get; }

    public IRepository<SolucaoProposta> Solucoes { get; }

    public IRepository<EntregavelGerado> Entregaveis { get; }

    public async Task<int> CompleteAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}