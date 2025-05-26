using DevInsight.Core.DTOs;

namespace DevInsight.Core.Interfaces.Services;

public interface IValidacaoTecnicaService
{
    Task<ValidacaoTecnicaConsultaDTO> CriarValidacaoAsync(ValidacaoTecnicaCriacaoDTO validacaoDto, Guid projetoId);
    Task<ValidacaoTecnicaConsultaDTO> ObterPorIdAsync(Guid id);
    Task<IEnumerable<ValidacaoTecnicaConsultaDTO>> ListarPorProjetoAsync(Guid projetoId);
    Task<ValidacaoTecnicaConsultaDTO> AtualizarValidacaoAsync(Guid id, ValidacaoTecnicaAtualizacaoDTO validacaoDto);
    Task<ValidacaoTecnicaConsultaDTO> MarcarComoValidadoAsync(Guid id, string? observacao);
    Task<bool> ExcluirValidacaoAsync(Guid id);
}