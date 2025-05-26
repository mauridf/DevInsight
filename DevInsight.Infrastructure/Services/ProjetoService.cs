using AutoMapper;
using DevInsight.Core.DTOs;
using DevInsight.Core.Entities;
using DevInsight.Core.Exceptions;
using DevInsight.Core.Interfaces;
using DevInsight.Core.Interfaces.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DevInsight.Infrastructure.Services;

public class ProjetoService : IProjetoService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<ProjetoService> _logger;

    public ProjetoService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<ProjetoService> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ProjetoConsultaDTO> CriarProjetoAsync(ProjetoCriacaoDTO projetoDto, Guid usuarioId)
    {
        try
        {
            var usuario = await _unitOfWork.Usuarios.GetByIdAsync(usuarioId);
            if (usuario == null)
            {
                _logger.LogWarning("Usuário não encontrado: {UsuarioId}", usuarioId);
                throw new NotFoundException("Usuário não encontrado");
            }

            var projeto = _mapper.Map<ProjetoConsultoria>(projetoDto);
            projeto.CriadoPorId = usuarioId;
            projeto.CriadoEm = DateTime.UtcNow;

            await _unitOfWork.Projetos.AddAsync(projeto);
            await _unitOfWork.CompleteAsync();

            _logger.LogInformation("Projeto criado com sucesso: {ProjetoId}", projeto.Id);
            return _mapper.Map<ProjetoConsultaDTO>(projeto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar projeto");
            throw;
        }
    }

    public async Task<ProjetoDetalhesDTO> ObterPorIdAsync(Guid id)
    {
        try
        {
            var projeto = await _unitOfWork.Projetos.GetByIdAsync(id);
            if (projeto == null)
            {
                _logger.LogWarning("Projeto não encontrado: {ProjetoId}", id);
                throw new NotFoundException("Projeto não encontrado");
            }

            // O mapeamento agora é completo pelo AutoMapper
            return _mapper.Map<ProjetoDetalhesDTO>(projeto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter projeto por ID: {ProjetoId}", id);
            throw;
        }
    }

    public async Task<IEnumerable<ProjetoConsultaDTO>> ListarTodosAsync()
    {
        try
        {
            // Carrega os projetos incluindo os dados do CriadoPor em uma única consulta
            var projetos = await _unitOfWork.Projetos.GetAllAsync(include: q => q.Include(p => p.CriadoPor));
            return _mapper.Map<IEnumerable<ProjetoConsultaDTO>>(projetos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar todos os projetos");
            throw;
        }
    }

    public async Task<ProjetoConsultaDTO> AtualizarProjetoAsync(Guid id, ProjetoAtualizacaoDTO projetoDto)
    {
        try
        {
            var projeto = await _unitOfWork.Projetos.GetByIdAsync(id);
            if (projeto == null)
            {
                _logger.LogWarning("Projeto não encontrado para atualização: {ProjetoId}", id);
                throw new NotFoundException("Projeto não encontrado");
            }

            _mapper.Map(projetoDto, projeto);
            await _unitOfWork.Projetos.UpdateAsync(projeto);
            await _unitOfWork.CompleteAsync();

            _logger.LogInformation("Projeto atualizado com sucesso: {ProjetoId}", id);
            return _mapper.Map<ProjetoConsultaDTO>(projeto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar projeto: {ProjetoId}", id);
            throw;
        }
    }

    public async Task<bool> ExcluirProjetoAsync(Guid id)
    {
        try
        {
            var projeto = await _unitOfWork.Projetos.GetByIdAsync(id);
            if (projeto == null)
            {
                _logger.LogWarning("Projeto não encontrado para exclusão: {ProjetoId}", id);
                throw new NotFoundException("Projeto não encontrado");
            }

            await _unitOfWork.Projetos.DeleteAsync(projeto);
            await _unitOfWork.CompleteAsync();

            _logger.LogInformation("Projeto excluído com sucesso: {ProjetoId}", id);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao excluir projeto: {ProjetoId}", id);
            throw;
        }
    }

    public async Task<IEnumerable<ProjetoConsultaDTO>> ListarPorUsuarioAsync(Guid usuarioId)
    {
        try
        {
            var usuario = await _unitOfWork.Usuarios.GetByIdAsync(usuarioId);
            if (usuario == null)
            {
                _logger.LogWarning("Usuário não encontrado: {UsuarioId}", usuarioId);
                throw new NotFoundException("Usuário não encontrado");
            }

            // Carrega os projetos incluindo os dados do CriadoPor em uma única consulta
            var projetos = await _unitOfWork.Projetos.GetAllAsync(
                filter: p => p.CriadoPorId == usuarioId,
                include: q => q.Include(p => p.CriadoPor));

            return _mapper.Map<IEnumerable<ProjetoConsultaDTO>>(projetos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar projetos por usuário: {UsuarioId}", usuarioId);
            throw;
        }
    }
}