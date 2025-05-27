using DevInsight.Core.DTOs;
using DevInsight.Core.Entities;

namespace DevInsight.Core.Services;

public interface ISolucaoPropostaService
{
    Task<SolucaoPropostaConsultaDTO> ObterPorIdAsync(Guid id);
    Task<IEnumerable<SolucaoPropostaConsultaDTO>> ListarPorProjetoAsync(Guid projetoId);
    Task<SolucaoPropostaConsultaDTO> CriarAsync(Guid projetoId, SolucaoPropostaCriacaoDTO dto);
    Task<SolucaoPropostaConsultaDTO> AtualizarAsync(Guid id, SolucaoPropostaAtualizacaoDTO dto);
    Task<bool> ExcluirAsync(Guid id);
}