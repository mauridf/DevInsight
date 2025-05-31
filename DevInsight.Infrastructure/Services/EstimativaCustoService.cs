using AutoMapper;
using DevInsight.Core.DTOs;
using DevInsight.Core.Entities;
using DevInsight.Core.Exceptions;
using DevInsight.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace DevInsight.Infrastructure.Services;

public class EstimativaCustoService : IEstimativaCusto
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<EstimativaCusto> _logger;

    public EstimativaCustoService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<EstimativaCusto> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<EstimativaCustoConsultaDTO> AtualizarEstimativaCustoAsync(Guid id, EstimativaCustoAtualizacaoDTO estimativaDto)
    {
        try
        {
            var estimativaCusto = await _unitOfWork.EstimativasCustos.GetByIdAsync(id);
            if (estimativaCusto == null)
            {
                _logger.LogWarning("Estimativa e Custo não encontrada para atualização: {EstimativaId}", id);
                throw new NotFoundException("Estimativa e Custo não encontrada");
            }

            _mapper.Map(estimativaDto, estimativaCusto);
            await _unitOfWork.EstimativasCustos.UpdateAsync(estimativaCusto);
            await _unitOfWork.CompleteAsync();

            _logger.LogInformation("Estimativa e Custo atualizada com sucesso: {EstimativaId}", id);
            return _mapper.Map<EstimativaCustoConsultaDTO>(estimativaCusto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar persona: {EstimativaId}", id);
            throw;
        }
    }

    public async Task<EstimativaCustoConsultaDTO> CriarEstimativaCustoAsync(EstimativaCustoCriacaoDTO estimativaDto, Guid projetoId)
    {
        try
        {
            var projeto = await _unitOfWork.Projetos.GetByIdAsync(projetoId);
            if (projeto == null)
            {
                _logger.LogWarning("Projeto não encontrado: {ProjetoId}", projetoId);
                throw new NotFoundException("Projeto não encontrado");
            }

            var estimativa = _mapper.Map<EstimativaCusto>(estimativaDto);
            estimativa.ProjetoId = projetoId;
            estimativa.CriadoEm = DateTime.UtcNow;

            await _unitOfWork.EstimativasCustos.AddAsync(estimativa);
            await _unitOfWork.CompleteAsync();

            _logger.LogInformation("Estimativa e Custo criada com sucesso: {EstimativaId}", estimativa.Id);
            return _mapper.Map<EstimativaCustoConsultaDTO>(estimativa);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar estimativa e custo de projeto");
            throw;
        }
    }

    public async Task<bool> ExcluirEstimativaCustoAsync(Guid id)
    {
        try
        {
            var estimativa = await _unitOfWork.EstimativasCustos.GetByIdAsync(id);
            if (estimativa == null)
            {
                _logger.LogWarning("Estimativa e Custo não encontrada para exclusão: {EstimativaId}", id);
                throw new NotFoundException("Estimativa e Custo não encontrada");
            }

            await _unitOfWork.EstimativasCustos.DeleteAsync(estimativa);
            await _unitOfWork.CompleteAsync();

            _logger.LogInformation("Estimativa e Custo excluída com sucesso: {EstimativaId}", id);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao excluir estimativa do projeto chave: {EstimativaId}", id);
            throw;
        }
    }

    public async Task<IEnumerable<EstimativaCustoConsultaDTO>> ListarPorProjetoAsync(Guid projetoId)
    {
        try
        {
            var projeto = await _unitOfWork.Projetos.GetByIdAsync(projetoId);
            if (projeto == null)
            {
                _logger.LogWarning("Projeto não encontrado: {ProjetoId}", projetoId);
                throw new NotFoundException("Projeto não encontrado");
            }

            var estimativas = (await _unitOfWork.EstimativasCustos.GetAllAsync())
                .Where(e => e.ProjetoId == projetoId)
                .ToList();

            return _mapper.Map<IEnumerable<EstimativaCustoConsultaDTO>>(estimativas);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar estimativas e custos por projeto: {ProjetoId}", projetoId);
            throw;
        }
    }

    public async Task<EstimativaCustoConsultaDTO> ObterPorIdAsync(Guid id)
    {
        try
        {
            var estimativa = await _unitOfWork.EstimativasCustos.GetByIdAsync(id);
            if (estimativa == null)
            {
                _logger.LogWarning("Estimativa e Custo não encontrada: {FaseId}", id);
                throw new NotFoundException("Estimativa e Custo não encontrada");
            }

            return _mapper.Map<EstimativaCustoConsultaDTO>(estimativa);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter estimativa e custo por ID: {FaseId}", id);
            throw;
        }
    }
}
