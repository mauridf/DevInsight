using DevInsight.Core.DTOs;

namespace DevInsight.Core.Interfaces.Services;

public interface IProjetoService
{
    Task<ProjetoConsultaDTO> CriarProjetoAsync(ProjetoCriacaoDTO projetoDto, Guid usuarioId);
    Task<ProjetoDetalhesDTO> ObterPorIdAsync(Guid id);
    Task<IEnumerable<ProjetoConsultaDTO>> ListarTodosAsync();
    Task<ProjetoConsultaDTO> AtualizarProjetoAsync(Guid id, ProjetoAtualizacaoDTO projetoDto);
    Task<bool> ExcluirProjetoAsync(Guid id);
    Task<IEnumerable<ProjetoConsultaDTO>> ListarPorUsuarioAsync(Guid usuarioId);
}