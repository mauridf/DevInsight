using AutoMapper;
using DevInsight.Core.DTOs;
using DevInsight.Core.Entities;
using DevInsight.Core.Exceptions;
using DevInsight.Core.Interfaces;
using DevInsight.Core.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace DevInsight.Infrastructure.Services;

public class DocumentoLinkService : IDocumentoLinkService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<DocumentoLinkService> _logger;

    public DocumentoLinkService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<DocumentoLinkService> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<DocumentoLinkConsultaDTO> CriarDocumentoLinkAsync(DocumentoLinkCriacaoDTO documentoLinkDto, Guid projetoId)
    {
        try
        {
            var projeto = await _unitOfWork.Projetos.GetByIdAsync(projetoId);
            if (projeto == null)
            {
                _logger.LogWarning("Projeto não encontrado: {ProjetoId}", projetoId);
                throw new NotFoundException("Projeto não encontrado");
            }

            var documentoLink = _mapper.Map<DocumentoLink>(documentoLinkDto);
            documentoLink.ProjetoId = projetoId;
            documentoLink.CriadoEm = DateTime.UtcNow;

            await _unitOfWork.Documentos.AddAsync(documentoLink);
            await _unitOfWork.CompleteAsync();

            _logger.LogInformation("DocumentoLink criado com sucesso: {DocumentoLinkId}", documentoLink.Id);
            return _mapper.Map<DocumentoLinkConsultaDTO>(documentoLink);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar DocumentoLink");
            throw;
        }
    }

    public async Task<DocumentoLinkConsultaDTO> ObterPorIdAsync(Guid id)
    {
        try
        {
            var documentoLink = await _unitOfWork.Documentos.GetByIdAsync(id);
            if (documentoLink == null)
            {
                _logger.LogWarning("DocumentoLink não encontrado: {DocumentoLinkId}", id);
                throw new NotFoundException("DocumentoLink não encontrado");
            }

            return _mapper.Map<DocumentoLinkConsultaDTO>(documentoLink);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter DocumentoLink por ID: {DocumentoLinkId}", id);
            throw;
        }
    }

    public async Task<IEnumerable<DocumentoLinkConsultaDTO>> ListarPorProjetoAsync(Guid projetoId)
    {
        try
        {
            var projeto = await _unitOfWork.Projetos.GetByIdAsync(projetoId);
            if (projeto == null)
            {
                _logger.LogWarning("Projeto não encontrado: {ProjetoId}", projetoId);
                throw new NotFoundException("Projeto não encontrado");
            }

            var documentosLinks = (await _unitOfWork.Documentos.GetAllAsync())
                .Where(d => d.ProjetoId == projetoId)
                .ToList();

            return _mapper.Map<IEnumerable<DocumentoLinkConsultaDTO>>(documentosLinks);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar DocumentosLinks por projeto: {ProjetoId}", projetoId);
            throw;
        }
    }

    public async Task<DocumentoLinkConsultaDTO> AtualizarDocumentoLinkAsync(Guid id, DocumentoLinkAtualizacaoDTO documentoLinkDto)
    {
        try
        {
            var documentoLink = await _unitOfWork.Documentos.GetByIdAsync(id);
            if (documentoLink == null)
            {
                _logger.LogWarning("DocumentoLink não encontrado para atualização: {DocumentoLinkId}", id);
                throw new NotFoundException("DocumentoLink não encontrado");
            }

            _mapper.Map(documentoLinkDto, documentoLink);
            await _unitOfWork.Documentos.UpdateAsync(documentoLink);
            await _unitOfWork.CompleteAsync();

            _logger.LogInformation("DocumentoLink atualizado com sucesso: {DocumentoLinkId}", id);
            return _mapper.Map<DocumentoLinkConsultaDTO>(documentoLink);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar DocumentoLink: {DocumentoLinkId}", id);
            throw;
        }
    }

    public async Task<bool> ExcluirDocumentoLinkAsync(Guid id)
    {
        try
        {
            var documentoLink = await _unitOfWork.Documentos.GetByIdAsync(id);
            if (documentoLink == null)
            {
                _logger.LogWarning("DocumentoLink não encontrado para exclusão: {DocumentoLinkId}", id);
                throw new NotFoundException("DocumentoLink não encontrado");
            }

            await _unitOfWork.Documentos.DeleteAsync(documentoLink);
            await _unitOfWork.CompleteAsync();

            _logger.LogInformation("DocumentoLink excluído com sucesso: {DocumentoLinkId}", id);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao excluir DocumentoLink: {DocumentoLinkId}", id);
            throw;
        }
    }
}