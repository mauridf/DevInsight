using AutoMapper;
using DevInsight.Core.DTOs;
using DevInsight.Core.Entities;
using DevInsight.Core.Exceptions;
using DevInsight.Core.Interfaces;
using DevInsight.Core.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace DevInsight.Infrastructure.Services;

public class StakeHolderService : IStakeHolderService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<StakeHolderService> _logger;

    public StakeHolderService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<StakeHolderService> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<StakeHolderConsultaDTO> CriarStakeHolderAsync(StakeHolderCriacaoDTO stakeHolderDto, Guid projetoId)
    {
        try
        {
            var projeto = await _unitOfWork.Projetos.GetByIdAsync(projetoId);
            if (projeto == null)
            {
                _logger.LogWarning("Projeto não encontrado: {ProjetoId}", projetoId);
                throw new NotFoundException("Projeto não encontrado");
            }

            var stakeHolder = _mapper.Map<StakeHolder>(stakeHolderDto);
            stakeHolder.ProjetoId = projetoId;
            stakeHolder.CriadoEm = DateTime.UtcNow;

            await _unitOfWork.StakeHolders.AddAsync(stakeHolder);
            await _unitOfWork.CompleteAsync();

            _logger.LogInformation("StakeHolder criado com sucesso: {StakeHolderId}", stakeHolder.Id);
            return _mapper.Map<StakeHolderConsultaDTO>(stakeHolder);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar StakeHolder");
            throw;
        }
    }

    public async Task<StakeHolderConsultaDTO> ObterPorIdAsync(Guid id)
    {
        try
        {
            var stakeHolder = await _unitOfWork.StakeHolders.GetByIdAsync(id);
            if (stakeHolder == null)
            {
                _logger.LogWarning("StakeHolder não encontrado: {StakeHolderId}", id);
                throw new NotFoundException("StakeHolder não encontrado");
            }

            return _mapper.Map<StakeHolderConsultaDTO>(stakeHolder);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter StakeHolder por ID: {StakeHolderId}", id);
            throw;
        }
    }

    public async Task<IEnumerable<StakeHolderConsultaDTO>> ListarPorProjetoAsync(Guid projetoId)
    {
        try
        {
            var projeto = await _unitOfWork.Projetos.GetByIdAsync(projetoId);
            if (projeto == null)
            {
                _logger.LogWarning("Projeto não encontrado: {ProjetoId}", projetoId);
                throw new NotFoundException("Projeto não encontrado");
            }

            var stakeHolders = (await _unitOfWork.StakeHolders.GetAllAsync())
                .Where(sh => sh.ProjetoId == projetoId)
                .ToList();

            return _mapper.Map<IEnumerable<StakeHolderConsultaDTO>>(stakeHolders);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar StakeHolders por projeto: {ProjetoId}", projetoId);
            throw;
        }
    }

    public async Task<StakeHolderConsultaDTO> AtualizarStakeHolderAsync(Guid id, StakeHolderAtualizacaoDTO stakeHolderDto)
    {
        try
        {
            var stakeHolder = await _unitOfWork.StakeHolders.GetByIdAsync(id);
            if (stakeHolder == null)
            {
                _logger.LogWarning("StakeHolder não encontrado para atualização: {StakeHolderId}", id);
                throw new NotFoundException("StakeHolder não encontrado");
            }

            _mapper.Map(stakeHolderDto, stakeHolder);
            _unitOfWork.StakeHolders.UpdateAsync(stakeHolder);
            await _unitOfWork.CompleteAsync();

            _logger.LogInformation("StakeHolder atualizado com sucesso: {StakeHolderId}", id);
            return _mapper.Map<StakeHolderConsultaDTO>(stakeHolder);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar StakeHolder: {StakeHolderId}", id);
            throw;
        }
    }

    public async Task<bool> ExcluirStakeHolderAsync(Guid id)
    {
        try
        {
            var stakeHolder = await _unitOfWork.StakeHolders.GetByIdAsync(id);
            if (stakeHolder == null)
            {
                _logger.LogWarning("StakeHolder não encontrado para exclusão: {StakeHolderId}", id);
                throw new NotFoundException("StakeHolder não encontrado");
            }

            await _unitOfWork.StakeHolders.DeleteAsync(stakeHolder);
            await _unitOfWork.CompleteAsync();

            _logger.LogInformation("StakeHolder excluído com sucesso: {StakeHolderId}", id);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao excluir StakeHolder: {StakeHolderId}", id);
            throw;
        }
    }
}