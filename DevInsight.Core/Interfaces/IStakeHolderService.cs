using DevInsight.Core.DTOs;

namespace DevInsight.Core.Interfaces.Services;

public interface IStakeHolderService
{
    Task<StakeHolderConsultaDTO> CriarStakeHolderAsync(StakeHolderCriacaoDTO stakeHolderDto, Guid projetoId);
    Task<StakeHolderConsultaDTO> ObterPorIdAsync(Guid id);
    Task<IEnumerable<StakeHolderConsultaDTO>> ListarPorProjetoAsync(Guid projetoId);
    Task<StakeHolderConsultaDTO> AtualizarStakeHolderAsync(Guid id, StakeHolderAtualizacaoDTO stakeHolderDto);
    Task<bool> ExcluirStakeHolderAsync(Guid id);
}