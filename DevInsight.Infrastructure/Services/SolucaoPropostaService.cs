using AutoMapper;
using DevInsight.Core.DTOs;
using DevInsight.Core.Entities;
using DevInsight.Core.Exceptions;
using DevInsight.Core.Interfaces;
using DevInsight.Core.Interfaces.Services;
using DevInsight.Infrastructure.Services;
using Microsoft.Extensions.Logging;

namespace DevInsight.Core.Services;

public class SolucaoPropostaService : ISolucaoPropostaService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<ReuniaoService> _logger;

    public SolucaoPropostaService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<ReuniaoService> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<SolucaoPropostaConsultaDTO> ObterPorIdAsync(Guid id)
    {
        try
        {
            var solucao = await _unitOfWork.Solucoes.GetByIdAsync(id);
            if (solucao == null)
            {
                _logger.LogWarning("Solução não encontrada: {SolucaoId}", id);
                throw new NotFoundException("Solução não encontrada");
            }

            return _mapper.Map<SolucaoPropostaConsultaDTO>(solucao);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter solução por ID: {SolucaoId}", id);
            throw;
        }
    }

    public async Task<IEnumerable<SolucaoPropostaConsultaDTO>> ListarPorProjetoAsync(Guid projetoId)
    {
        try
        {
            var projeto = await _unitOfWork.Projetos.GetByIdAsync(projetoId);
            if (projeto == null)
            {
                _logger.LogWarning("Projeto não encontrado: {ProjetoId}", projetoId);
                throw new NotFoundException("Projeto não encontrado");
            }

            var solucoes = (await _unitOfWork.Solucoes.GetAllAsync())
                .Where(s => s.ProjetoId == projetoId)
                .ToList();

            return _mapper.Map<IEnumerable<SolucaoPropostaConsultaDTO>>(solucoes);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar soluções por projeto: {ProjetoId}", projetoId);
            throw;
        }
    }

    public async Task<SolucaoPropostaConsultaDTO> CriarAsync(Guid projetoId, SolucaoPropostaCriacaoDTO solucaoDto)
    {
        try
        {
            var projeto = await _unitOfWork.Projetos.GetByIdAsync(projetoId);
            if (projeto == null)
            {
                _logger.LogWarning("Projeto não encontrado: {ProjetoId}", projetoId);
                throw new NotFoundException("Projeto não encontrado");
            }

            var solucao = _mapper.Map<SolucaoProposta>(solucaoDto);
            solucao.ProjetoId = projetoId;
            solucao.CriadoEm = DateTime.UtcNow;

            await _unitOfWork.Solucoes.AddAsync(solucao);
            await _unitOfWork.CompleteAsync();

            _logger.LogInformation("Solução Proposta criada com sucesso: {SolucaoId}", solucao.Id);
            return _mapper.Map<SolucaoPropostaConsultaDTO>(solucao);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar solução");
            throw;
        }
    }

    public async Task<SolucaoPropostaConsultaDTO> AtualizarAsync(Guid id, SolucaoPropostaAtualizacaoDTO solucaoDto)
    {
        try
        {
            var solucao = await _unitOfWork.Solucoes.GetByIdAsync(id);
            if (solucao == null)
            {
                _logger.LogWarning("Solução não encontrada para atualização: {SolucaoId}", id);
                throw new NotFoundException("Solução não encontrada");
            }

            _mapper.Map(solucaoDto, solucao);
            await _unitOfWork.Solucoes.UpdateAsync(solucao);
            await _unitOfWork.CompleteAsync();

            _logger.LogInformation("Solução atualizada com sucesso: {SolucaoId}", id);
            return _mapper.Map<SolucaoPropostaConsultaDTO>(solucao);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar solução: {SolucaoId}", id);
            throw;
        }
    }

    public async Task<bool> ExcluirAsync(Guid id)
    {
        try
        {
            var solucao = await _unitOfWork.Solucoes.GetByIdAsync(id);
            if (solucao == null)
            {
                _logger.LogWarning("Solução não encontrada para exclusão: {SolucaoId}", id);
                throw new NotFoundException("Solução não encontrada");
            }

            await _unitOfWork.Solucoes.DeleteAsync(solucao);
            await _unitOfWork.CompleteAsync();

            _logger.LogInformation("Solução excluída com sucesso: {SolucaoId}", id);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao excluir solução: {SolucaoId}", id);
            throw;
        }
    }
}