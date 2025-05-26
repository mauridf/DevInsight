using DevInsight.Core.DTOs;

namespace DevInsight.Core.Interfaces.Services;

public interface IEntregaFinalService
{
    Task<EntregaFinalConsultaDTO> CriarEntregaAsync(EntregaFinalCriacaoDTO entregaDto, Guid projetoId);
    Task<EntregaFinalConsultaDTO> ObterPorIdAsync(Guid id);
    Task<IEnumerable<EntregaFinalConsultaDTO>> ListarPorProjetoAsync(Guid projetoId);
    Task<EntregaFinalConsultaDTO> AtualizarEntregaAsync(Guid id, EntregaFinalAtualizacaoDTO entregaDto);
    Task<bool> ExcluirEntregaAsync(Guid id);
}