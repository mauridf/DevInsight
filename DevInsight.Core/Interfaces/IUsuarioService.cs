using DevInsight.Core.DTOs;

namespace DevInsight.Core.Interfaces;

public interface IUsuarioService
{
    Task<UsuarioConsultaDTO> ObterPorIdAsync(Guid id);
    Task<UsuarioConsultaDTO> ObterPorEmailAsync(string email);
    Task<IEnumerable<UsuarioConsultaDTO>> ListarTodosAsync();
    Task<UsuarioConsultaDTO> AtualizarAsync(Guid id, UsuarioAtualizacaoDTO atualizacaoDto);
    Task<bool> ExcluirAsync(Guid id);
}