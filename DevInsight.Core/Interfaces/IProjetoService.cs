using DevInsight.Core.DTOs;
using DevInsight.Core.Entities;

namespace DevInsight.Core.Interfaces.Services;

public interface IProjetoService
{
    Task<ProjetoConsultaDTO> CriarProjetoAsync(ProjetoCriacaoDTO projetoDto, Guid usuarioId);
    Task<ProjetoDetalhesDTO> ObterPorIdAsync(Guid id);
    Task<IEnumerable<ProjetoConsultaDTO>> ListarTodosAsync();
    Task<ProjetoConsultaDTO> AtualizarProjetoAsync(Guid id, ProjetoAtualizacaoDTO projetoDto);
    Task<bool> ExcluirProjetoAsync(Guid id);
    Task<IEnumerable<ProjetoConsultaDTO>> ListarPorUsuarioAsync(Guid usuarioId);
    Task<DashboardDTO> ObterDadosDashboardAsync(Guid usuarioId);
    Task<ProjetoConsultoria> ObterProjetoCompleto(Guid projetoId);
    Task<IEnumerable<ValidacaoTecnica>> ObterValidacoesTecnicas(Guid projetoId);
    Task<IEnumerable<EntregaFinal>> ObterEntregasFinais(Guid projetoId);
    Task<IEnumerable<TarefaProjeto>> ObterTarefasKanban(Guid projetoId);
    Task<SolucaoProposta> ObterSolucaoProposta(Guid projetoId);
}