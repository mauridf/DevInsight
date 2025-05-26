using AutoMapper;
using DevInsight.Core.DTOs;
using DevInsight.Core.Entities;
using DevInsight.Core.Exceptions;
using DevInsight.Core.Interfaces;
using DevInsight.Core.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace DevInsight.Infrastructure.Services;

public class EntregaFinalService : IEntregaFinalService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<EntregaFinalService> _logger;

    public EntregaFinalService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<EntregaFinalService> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<EntregaFinalConsultaDTO> CriarEntregaAsync(EntregaFinalCriacaoDTO entregaDto, Guid projetoId)
    {
        try
        {
            var projeto = await _unitOfWork.Projetos.GetByIdAsync(projetoId);
            if (projeto == null)
            {
                _logger.LogWarning("Projeto não encontrado: {ProjetoId}", projetoId);
                throw new NotFoundException("Projeto não encontrado");
            }

            var entrega = _mapper.Map<EntregaFinal>(entregaDto);
            entrega.ProjetoId = projetoId;
            entrega.CriadoEm = DateTime.UtcNow;

            await _unitOfWork.Entregas.AddAsync(entrega);
            await _unitOfWork.CompleteAsync();

            _logger.LogInformation("Entrega final criada com sucesso: {EntregaId}", entrega.Id);
            return _mapper.Map<EntregaFinalConsultaDTO>(entrega);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar entrega final");
            throw;
        }
    }

    public async Task<EntregaFinalConsultaDTO> ObterPorIdAsync(Guid id)
    {
        try
        {
            var entrega = await _unitOfWork.Entregas.GetByIdAsync(id);
            if (entrega == null)
            {
                _logger.LogWarning("Entrega final não encontrada: {EntregaId}", id);
                throw new NotFoundException("Entrega final não encontrada");
            }

            return _mapper.Map<EntregaFinalConsultaDTO>(entrega);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter entrega final por ID: {EntregaId}", id);
            throw;
        }
    }

    public async Task<IEnumerable<EntregaFinalConsultaDTO>> ListarPorProjetoAsync(Guid projetoId)
    {
        try
        {
            var projeto = await _unitOfWork.Projetos.GetByIdAsync(projetoId);
            if (projeto == null)
            {
                _logger.LogWarning("Projeto não encontrado: {ProjetoId}", projetoId);
                throw new NotFoundException("Projeto não encontrado");
            }

            var entregas = (await _unitOfWork.Entregas.GetAllAsync())
                .Where(e => e.ProjetoId == projetoId)
                .OrderByDescending(e => e.CriadoEm)
                .ToList();

            return _mapper.Map<IEnumerable<EntregaFinalConsultaDTO>>(entregas);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar entregas finais por projeto: {ProjetoId}", projetoId);
            throw;
        }
    }

    public async Task<EntregaFinalConsultaDTO> AtualizarEntregaAsync(Guid id, EntregaFinalAtualizacaoDTO entregaDto)
    {
        try
        {
            var entrega = await _unitOfWork.Entregas.GetByIdAsync(id);
            if (entrega == null)
            {
                _logger.LogWarning("Entrega final não encontrada para atualização: {EntregaId}", id);
                throw new NotFoundException("Entrega final não encontrada");
            }

            _mapper.Map(entregaDto, entrega);
            await _unitOfWork.Entregas.UpdateAsync(entrega);
            await _unitOfWork.CompleteAsync();

            _logger.LogInformation("Entrega final atualizada com sucesso: {EntregaId}", id);
            return _mapper.Map<EntregaFinalConsultaDTO>(entrega);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar entrega final: {EntregaId}", id);
            throw;
        }
    }

    public async Task<bool> ExcluirEntregaAsync(Guid id)
    {
        try
        {
            var entrega = await _unitOfWork.Entregas.GetByIdAsync(id);
            if (entrega == null)
            {
                _logger.LogWarning("Entrega final não encontrada para exclusão: {EntregaId}", id);
                throw new NotFoundException("Entrega final não encontrada");
            }

            await _unitOfWork.Entregas.DeleteAsync(entrega);
            await _unitOfWork.CompleteAsync();

            _logger.LogInformation("Entrega final excluída com sucesso: {EntregaId}", id);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao excluir entrega final: {EntregaId}", id);
            throw;
        }
    }
}