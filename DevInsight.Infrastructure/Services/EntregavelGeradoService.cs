using AutoMapper;
using DevInsight.Core.DTOs;
using DevInsight.Core.Entities;
using DevInsight.Core.Exceptions;
using DevInsight.Core.Interfaces;
using DevInsight.Core.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace DevInsight.Infrastructure.Services;

public class EntregavelGeradoService : IEntregavelGeradoService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<EntregavelGeradoService> _logger;
    private readonly IStorageService _storageService;

    public EntregavelGeradoService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<EntregavelGeradoService> logger,
        IStorageService storageService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
        _storageService = storageService;
    }

    public async Task<EntregavelGeradoConsultaDTO> CriarEntregavelAsync(EntregavelGeradoCriacaoDTO entregavelDto, Guid projetoId)
    {
        try
        {
            var projeto = await _unitOfWork.Projetos.GetByIdAsync(projetoId);
            if (projeto == null)
            {
                _logger.LogWarning("Projeto não encontrado: {ProjetoId}", projetoId);
                throw new NotFoundException("Projeto não encontrado");
            }

            var entregavel = _mapper.Map<EntregavelGerado>(entregavelDto);
            entregavel.ProjetoId = projetoId;
            entregavel.CriadoEm = DateTime.UtcNow;

            await _unitOfWork.Entregaveis.AddAsync(entregavel);
            await _unitOfWork.CompleteAsync();

            _logger.LogInformation("Entregável criado com sucesso: {EntregavelId}", entregavel.Id);
            return _mapper.Map<EntregavelGeradoConsultaDTO>(entregavel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar entregável");
            throw;
        }
    }

    public async Task<EntregavelGeradoConsultaDTO> ObterPorIdAsync(Guid id)
    {
        try
        {
            var entregavel = await _unitOfWork.Entregaveis.GetByIdAsync(id);
            if (entregavel == null)
            {
                _logger.LogWarning("Entregável não encontrado: {EntregavelId}", id);
                throw new NotFoundException("Entregável não encontrado");
            }

            var dto = _mapper.Map<EntregavelGeradoConsultaDTO>(entregavel);
            dto.UrlDownload = await _storageService.GetFileUrlAsync($"entregaveis/{entregavel.Id}");

            return dto;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter entregável por ID: {EntregavelId}", id);
            throw;
        }
    }

    public async Task<IEnumerable<EntregavelGeradoConsultaDTO>> ListarPorProjetoAsync(Guid projetoId)
    {
        try
        {
            var projeto = await _unitOfWork.Projetos.GetByIdAsync(projetoId);
            if (projeto == null)
            {
                _logger.LogWarning("Projeto não encontrado: {ProjetoId}", projetoId);
                throw new NotFoundException("Projeto não encontrado");
            }

            var entregaveis = (await _unitOfWork.Entregaveis.GetAllAsync())
                .Where(e => e.ProjetoId == projetoId)
                .OrderByDescending(e => e.CriadoEm)
                .ToList();

            var dtos = _mapper.Map<IEnumerable<EntregavelGeradoConsultaDTO>>(entregaveis);

            // Gerar URLs de download para cada entregável
            foreach (var dto in dtos)
            {
                dto.UrlDownload = await _storageService.GetFileUrlAsync($"entregaveis/{dto.Id}");
            }

            return dtos;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar entregáveis por projeto: {ProjetoId}", projetoId);
            throw;
        }
    }

    public async Task<bool> ExcluirEntregavelAsync(Guid id)
    {
        try
        {
            var entregavel = await _unitOfWork.Entregaveis.GetByIdAsync(id);
            if (entregavel == null)
            {
                _logger.LogWarning("Entregável não encontrado para exclusão: {EntregavelId}", id);
                throw new NotFoundException("Entregável não encontrado");
            }

            await _unitOfWork.Entregaveis.DeleteAsync(entregavel);
            await _unitOfWork.CompleteAsync();

            // Remover arquivo associado no storage
            await _storageService.DeleteFileAsync($"entregaveis/{id}");

            _logger.LogInformation("Entregável excluído com sucesso: {EntregavelId}", id);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao excluir entregável: {EntregavelId}", id);
            throw;
        }
    }

    public async Task<string> GerarUrlDownloadAsync(Guid id)
    {
        try
        {
            var entregavel = await _unitOfWork.Entregaveis.GetByIdAsync(id);
            if (entregavel == null)
            {
                _logger.LogWarning("Entregável não encontrado para gerar URL: {EntregavelId}", id);
                throw new NotFoundException("Entregável não encontrado");
            }

            var url = await _storageService.GetFileUrlAsync($"entregaveis/{id}");
            return url;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao gerar URL de download: {EntregavelId}", id);
            throw;
        }
    }
}