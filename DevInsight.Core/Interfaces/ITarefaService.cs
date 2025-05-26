using DevInsight.Core.DTOs;
using DevInsight.Core.Enums;

namespace DevInsight.Core.Interfaces.Services;

public interface ITarefaService
{
    Task<TarefaConsultaDTO> CriarTarefaAsync(TarefaCriacaoDTO tarefaDto, Guid projetoId);
    Task<TarefaConsultaDTO> ObterPorIdAsync(Guid id);
    Task<IEnumerable<TarefaConsultaDTO>> ListarPorProjetoAsync(Guid projetoId);
    Task<IEnumerable<TarefaKanbanDTO>> ListarParaKanbanAsync(Guid projetoId);
    Task<TarefaConsultaDTO> AtualizarTarefaAsync(Guid id, TarefaAtualizacaoDTO tarefaDto);
    Task<TarefaConsultaDTO> AtualizarStatusAsync(Guid id, StatusTarefa status);
    Task<bool> ExcluirTarefaAsync(Guid id);
}