using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DevInsight.Core.DTOs;
using DevInsight.Core.Entities;
using DevInsight.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DevInsight.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _config;

    public AuthService(IUnitOfWork unitOfWork, IConfiguration config)
    {
        _unitOfWork = unitOfWork;
        _config = config;
    }

    public async Task<UsuarioRespostaDto> Registrar(UsuarioRegistroDto registroDto)
    {
        // Verificar se email já existe
        var usuarioExistente = (await _unitOfWork.Usuarios.GetAllAsync())
            .FirstOrDefault(u => u.Email == registroDto.Email);

        if (usuarioExistente != null)
            throw new Exception("Email já está em uso");

        // Criar novo usuário
        var novoUsuario = new Usuario
        {
            Nome = registroDto.Nome,
            Email = registroDto.Email,
            SenhaHash = BCrypt.Net.BCrypt.HashPassword(registroDto.Senha),
            TipoUsuario = registroDto.TipoUsuario,
            EmailConfirmado = false,
            CriadoEm = DateTime.UtcNow
        };

        await _unitOfWork.Usuarios.AddAsync(novoUsuario);
        await _unitOfWork.CompleteAsync();

        // Gerar token
        var token = GenerateJwtToken(novoUsuario);

        return new UsuarioRespostaDto
        {
            Id = novoUsuario.Id,
            Nome = novoUsuario.Nome,
            Email = novoUsuario.Email,
            TipoUsuario = novoUsuario.TipoUsuario,
            Token = token
        };
    }

    public async Task<UsuarioRespostaDto> Login(LoginDto loginDto)
    {
        var usuario = (await _unitOfWork.Usuarios.GetAllAsync())
            .FirstOrDefault(u => u.Email == loginDto.Email);

        if (usuario == null || !BCrypt.Net.BCrypt.Verify(loginDto.Senha, usuario.SenhaHash))
            throw new Exception("Email ou senha incorretos");

        // Gerar token
        var token = GenerateJwtToken(usuario);

        return new UsuarioRespostaDto
        {
            Id = usuario.Id,
            Nome = usuario.Nome,
            Email = usuario.Email,
            TipoUsuario = usuario.TipoUsuario,
            Token = token
        };
    }

    private string GenerateJwtToken(Usuario usuario)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            new Claim(ClaimTypes.Email, usuario.Email),
            new Claim(ClaimTypes.Role, usuario.TipoUsuario.ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(Convert.ToInt32(_config["Jwt:ExpiryInMinutes"])),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}