using DevInsight.Core.DTOs;

namespace DevInsight.Core.Interfaces.Services;

public interface IReuniaoService
{
    Task<ReuniaoConsultaDTO> CriarReuniaoAsync(ReuniaoCriacaoDTO reuniaoDto, Guid projetoId);
    Task<ReuniaoConsultaDTO> ObterPorIdAsync(Guid id);
    Task<IEnumerable<ReuniaoConsultaDTO>> ListarPorProjetoAsync(Guid projetoId);
    Task<ReuniaoConsultaDTO> AtualizarReuniaoAsync(Guid id, ReuniaoAtualizacaoDTO reuniaoDto);
    Task<bool> ExcluirReuniaoAsync(Guid id);
    Task<IEnumerable<ReuniaoConsultaDTO>> ListarProximasReunioesAsync(int dias = 7);
}