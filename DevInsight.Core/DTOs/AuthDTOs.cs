using DevInsight.Core.Enums;

namespace DevInsight.Core.DTOs;

public class LoginDto
{
    public string Email { get; set; } = null!;
    public string Senha { get; set; } = null!;
}

public class UsuarioRegistroDto
{
    public string Nome { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Senha { get; set; } = null!;
    public TipoUsuario TipoUsuario { get; set; }
}

public class UsuarioRespostaDto
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = null!;
    public string Email { get; set; } = null!;
    public TipoUsuario TipoUsuario { get; set; }
    public string Token { get; set; } = null!;
}

public class UsuarioConsultaDTO
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = null!;
    public string Email { get; set; } = null!;
    public TipoUsuario TipoUsuario { get; set; }
    public bool EmailConfirmado { get; set; }
    public DateTime CriadoEm { get; set; }
}
