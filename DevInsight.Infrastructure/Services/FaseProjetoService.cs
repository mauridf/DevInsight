using AutoMapper;
using DevInsight.Core.DTOs;
using DevInsight.Core.Entities;
using DevInsight.Core.Exceptions;
using DevInsight.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace DevInsight.Infrastructure.Services;

public class FaseProjetoService : IFaseProjetoService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<FaseProjeto> _logger;

    public FaseProjetoService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<FaseProjeto> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<FaseProjetoConsultaDTO> AtualizarFaseProjetoAsync(Guid id, FaseProjetoAtualizacaoDTO faseDto)
    {
        try
        {
            var fase = await _unitOfWork.FasesProjeto.GetByIdAsync(id);
            if (fase == null)
            {
                _logger.LogWarning("Fase de Projeto não encontrada para atualização: {FaseId}", id);
                throw new NotFoundException("Fase de Projeto não encontrada");
            }

            _mapper.Map(faseDto, fase);
            await _unitOfWork.FasesProjeto.UpdateAsync(fase);
            await _unitOfWork.CompleteAsync();

            _logger.LogInformation("Fase de Projeto atualizada com sucesso: {FaseId}", id);
            return _mapper.Map<FaseProjetoConsultaDTO>(fase);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar persona: {FaseId}", id);
            throw;
        }
    }

    public async Task<FaseProjetoConsultaDTO> CriarFaseProjetoAsync(FaseProjetoCriacaoDTO faseDto, Guid projetoId)
    {
        try
        {
            var projeto = await _unitOfWork.Projetos.GetByIdAsync(projetoId);
            if (projeto == null)
            {
                _logger.LogWarning("Projeto não encontrado: {ProjetoId}", projetoId);
                throw new NotFoundException("Projeto não encontrado");
            }

            var fase = _mapper.Map<FaseProjeto>(faseDto);
            fase.ProjetoId = projetoId;
            fase.CriadoEm = DateTime.UtcNow;

            await _unitOfWork.FasesProjeto.AddAsync(fase);
            await _unitOfWork.CompleteAsync();

            _logger.LogInformation("Fase de Projeto criada com sucesso: {FaseId}", fase.Id);
            return _mapper.Map<FaseProjetoConsultaDTO>(fase);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar fase de projeto");
            throw;
        }
    }

    public async Task<bool> ExcluirFaseProjetoAsync(Guid id)
    {
        try
        {
            var fase = await _unitOfWork.FasesProjeto.GetByIdAsync(id);
            if (fase == null)
            {
                _logger.LogWarning("Fase do Projeto não encontrada para exclusão: {FaseId}", id);
                throw new NotFoundException("Fase do Projeto não encontrada");
            }

            await _unitOfWork.FasesProjeto.DeleteAsync(fase);
            await _unitOfWork.CompleteAsync();

            _logger.LogInformation("Fase do Projeto excluída com sucesso: {FaseId}", id);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao excluir fase do proejeto chave: {FaseId}", id);
            throw;
        }
    }

    public async Task<IEnumerable<FaseProjetoConsultaDTO>> ListarPorProjetoAsync(Guid projetoId)
    {
        try
        {
            var projeto = await _unitOfWork.Projetos.GetByIdAsync(projetoId);
            if (projeto == null)
            {
                _logger.LogWarning("Projeto não encontrado: {ProjetoId}", projetoId);
                throw new NotFoundException("Projeto não encontrado");
            }

            var fases = (await _unitOfWork.FasesProjeto.GetAllAsync())
                .Where(f => f.ProjetoId == projetoId)
                .ToList();

            return _mapper.Map<IEnumerable<FaseProjetoConsultaDTO>>(fases);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar fases do projeto por projeto: {ProjetoId}", projetoId);
            throw;
        }
    }

    public async Task<FaseProjetoConsultaDTO> ObterPorIdAsync(Guid id)
    {
        try
        {
            var fase = await _unitOfWork.FasesProjeto.GetByIdAsync(id);
            if (fase == null)
            {
                _logger.LogWarning("Fase do Projeto não encontrada: {FaseId}", id);
                throw new NotFoundException("Fase do Projeto não encontrada");
            }

            return _mapper.Map<FaseProjetoConsultaDTO>(fase);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter fase do projeto por ID: {FaseId}", id);
            throw;
        }
    }
}
