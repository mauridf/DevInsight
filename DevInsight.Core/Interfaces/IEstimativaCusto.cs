using DevInsight.Core.DTOs;

namespace DevInsight.Core.Interfaces;

public interface IEstimativaCusto
{
    Task<EstimativaCustoConsultaDTO> CriarEstimativaCustoAsync(EstimativaCustoCriacaoDTO estimativaDto, Guid projetoId);
    Task<EstimativaCustoConsultaDTO> ObterPorIdAsync(Guid id);
    Task<IEnumerable<EstimativaCustoConsultaDTO>> ListarPorProjetoAsync(Guid projetoId);
    Task<EstimativaCustoConsultaDTO> AtualizarEstimativaCustoAsync(Guid id, EstimativaCustoAtualizacaoDTO estimativaDto);
    Task<bool> ExcluirEstimativaCustoAsync(Guid id);
}
