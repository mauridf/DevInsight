using DevInsight.Core.DTOs;

namespace DevInsight.Core.Interfaces;

public interface IAuthService
{
    Task<UsuarioRespostaDto> Registrar(UsuarioRegistroDto registroDto);
    Task<UsuarioRespostaDto> Login(LoginDto loginDto);
}
