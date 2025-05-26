using AutoMapper;
using DevInsight.Core.DTOs;
using DevInsight.Core.Entities;
using DevInsight.Core.Exceptions;
using DevInsight.Core.Interfaces;
using DevInsight.Core.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace DevInsight.Infrastructure.Services;

public class ValidacaoTecnicaService : IValidacaoTecnicaService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<ValidacaoTecnicaService> _logger;

    public ValidacaoTecnicaService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<ValidacaoTecnicaService> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ValidacaoTecnicaConsultaDTO> CriarValidacaoAsync(ValidacaoTecnicaCriacaoDTO validacaoDto, Guid projetoId)
    {
        try
        {
            var projeto = await _unitOfWork.Projetos.GetByIdAsync(projetoId);
            if (projeto == null)
            {
                _logger.LogWarning("Projeto não encontrado: {ProjetoId}", projetoId);
                throw new NotFoundException("Projeto não encontrado");
            }

            var validacao = _mapper.Map<ValidacaoTecnica>(validacaoDto);
            validacao.ProjetoId = projetoId;
            validacao.Validado = false; // Nova validação começa como não validada
            validacao.CriadoEm = DateTime.UtcNow;

            await _unitOfWork.ValidacoesTecnicas.AddAsync(validacao);
            await _unitOfWork.CompleteAsync();

            _logger.LogInformation("Validação técnica criada com sucesso: {ValidacaoId}", validacao.Id);
            return _mapper.Map<ValidacaoTecnicaConsultaDTO>(validacao);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar validação técnica");
            throw;
        }
    }

    public async Task<ValidacaoTecnicaConsultaDTO> ObterPorIdAsync(Guid id)
    {
        try
        {
            var validacao = await _unitOfWork.ValidacoesTecnicas.GetByIdAsync(id);
            if (validacao == null)
            {
                _logger.LogWarning("Validação técnica não encontrada: {ValidacaoId}", id);
                throw new NotFoundException("Validação técnica não encontrada");
            }

            return _mapper.Map<ValidacaoTecnicaConsultaDTO>(validacao);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter validação técnica por ID: {ValidacaoId}", id);
            throw;
        }
    }

    public async Task<IEnumerable<ValidacaoTecnicaConsultaDTO>> ListarPorProjetoAsync(Guid projetoId)
    {
        try
        {
            var projeto = await _unitOfWork.Projetos.GetByIdAsync(projetoId);
            if (projeto == null)
            {
                _logger.LogWarning("Projeto não encontrado: {ProjetoId}", projetoId);
                throw new NotFoundException("Projeto não encontrado");
            }

            var validacoes = (await _unitOfWork.ValidacoesTecnicas.GetAllAsync())
                .Where(v => v.ProjetoId == projetoId)
                .OrderByDescending(v => v.CriadoEm)
                .ToList();

            return _mapper.Map<IEnumerable<ValidacaoTecnicaConsultaDTO>>(validacoes);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar validações técnicas por projeto: {ProjetoId}", projetoId);
            throw;
        }
    }

    public async Task<ValidacaoTecnicaConsultaDTO> AtualizarValidacaoAsync(Guid id, ValidacaoTecnicaAtualizacaoDTO validacaoDto)
    {
        try
        {
            var validacao = await _unitOfWork.ValidacoesTecnicas.GetByIdAsync(id);
            if (validacao == null)
            {
                _logger.LogWarning("Validação técnica não encontrada para atualização: {ValidacaoId}", id);
                throw new NotFoundException("Validação técnica não encontrada");
            }

            _mapper.Map(validacaoDto, validacao);
            await _unitOfWork.ValidacoesTecnicas.UpdateAsync(validacao);
            await _unitOfWork.CompleteAsync();

            _logger.LogInformation("Validação técnica atualizada com sucesso: {ValidacaoId}", id);
            return _mapper.Map<ValidacaoTecnicaConsultaDTO>(validacao);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar validação técnica: {ValidacaoId}", id);
            throw;
        }
    }

    public async Task<ValidacaoTecnicaConsultaDTO> MarcarComoValidadoAsync(Guid id, string? observacao)
    {
        try
        {
            var validacao = await _unitOfWork.ValidacoesTecnicas.GetByIdAsync(id);
            if (validacao == null)
            {
                _logger.LogWarning("Validação técnica não encontrada para marcação como validado: {ValidacaoId}", id);
                throw new NotFoundException("Validação técnica não encontrada");
            }

            validacao.Validado = true;
            validacao.Observacao = observacao;
            await _unitOfWork.ValidacoesTecnicas.UpdateAsync(validacao);
            await _unitOfWork.CompleteAsync();

            _logger.LogInformation("Validação técnica marcada como validada: {ValidacaoId}", id);
            return _mapper.Map<ValidacaoTecnicaConsultaDTO>(validacao);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao marcar validação técnica como validada: {ValidacaoId}", id);
            throw;
        }
    }

    public async Task<bool> ExcluirValidacaoAsync(Guid id)
    {
        try
        {
            var validacao = await _unitOfWork.ValidacoesTecnicas.GetByIdAsync(id);
            if (validacao == null)
            {
                _logger.LogWarning("Validação técnica não encontrada para exclusão: {ValidacaoId}", id);
                throw new NotFoundException("Validação técnica não encontrada");
            }

            await _unitOfWork.ValidacoesTecnicas.DeleteAsync(validacao);
            await _unitOfWork.CompleteAsync();

            _logger.LogInformation("Validação técnica excluída com sucesso: {ValidacaoId}", id);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao excluir validação técnica: {ValidacaoId}", id);
            throw;
        }
    }
}