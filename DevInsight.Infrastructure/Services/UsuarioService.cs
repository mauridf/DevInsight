using DevInsight.Core.DTOs;
using DevInsight.Core.Entities;
using DevInsight.Core.Exceptions;
using DevInsight.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace DevInsight.Infrastructure.Services;

public class UsuarioService : IUsuarioService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UsuarioService> _logger;

    public UsuarioService(IUnitOfWork unitOfWork, ILogger<UsuarioService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<UsuarioConsultaDTO> ObterPorIdAsync(Guid id)
    {
        try
        {
            var usuario = await _unitOfWork.Usuarios.GetByIdAsync(id);
            if (usuario == null)
                throw new NotFoundException("Usuário não encontrado");

            return MapToConsultaDTO(usuario);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter usuário por ID: {Id}", id);
            throw;
        }
    }

    public async Task<UsuarioConsultaDTO> ObterPorEmailAsync(string email)
    {
        try
        {
            var usuario = (await _unitOfWork.Usuarios.GetAllAsync())
                .FirstOrDefault(u => u.Email == email);

            if (usuario == null)
                throw new NotFoundException("Usuário não encontrado");

            return MapToConsultaDTO(usuario);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter usuário por email: {Email}", email);
            throw;
        }
    }

    public async Task<IEnumerable<UsuarioConsultaDTO>> ListarTodosAsync()
    {
        try
        {
            var usuarios = await _unitOfWork.Usuarios.GetAllAsync();
            return usuarios.Select(MapToConsultaDTO);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar usuários");
            throw;
        }
    }

    public async Task<UsuarioConsultaDTO> AtualizarAsync(Guid id, UsuarioAtualizacaoDTO atualizacaoDto)
    {
        try
        {
            var usuario = await _unitOfWork.Usuarios.GetByIdAsync(id);
            if (usuario == null)
                throw new NotFoundException("Usuário não encontrado");

            usuario.Nome = atualizacaoDto.Nome;

            if (!string.IsNullOrEmpty(atualizacaoDto.Senha))
            {
                usuario.SenhaHash = BCrypt.Net.BCrypt.HashPassword(atualizacaoDto.Senha);
            }

            await _unitOfWork.Usuarios.UpdateAsync(usuario);
            await _unitOfWork.CompleteAsync();

            return MapToConsultaDTO(usuario);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar usuário: {Id}", id);
            throw;
        }
    }

    public async Task<bool> ExcluirAsync(Guid id)
    {
        try
        {
            var usuario = await _unitOfWork.Usuarios.GetByIdAsync(id);
            if (usuario == null)
                throw new NotFoundException("Usuário não encontrado");

            await _unitOfWork.Usuarios.DeleteAsync(usuario);
            await _unitOfWork.CompleteAsync();

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao excluir usuário: {Id}", id);
            throw;
        }
    }

    private static UsuarioConsultaDTO MapToConsultaDTO(Usuario usuario)
    {
        return new UsuarioConsultaDTO
        {
            Id = usuario.Id,
            Nome = usuario.Nome,
            Email = usuario.Email,
            TipoUsuario = usuario.TipoUsuario,
            EmailConfirmado = usuario.EmailConfirmado,
            CriadoEm = usuario.CriadoEm
        };
    }
}