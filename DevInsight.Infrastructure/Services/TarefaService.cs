using AutoMapper;
using DevInsight.Core.DTOs;
using DevInsight.Core.Entities;
using DevInsight.Core.Enums;
using DevInsight.Core.Exceptions;
using DevInsight.Core.Interfaces;
using DevInsight.Core.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace DevInsight.Infrastructure.Services;

public class TarefaService : ITarefaService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<TarefaService> _logger;

    public TarefaService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<TarefaService> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<TarefaConsultaDTO> CriarTarefaAsync(TarefaCriacaoDTO tarefaDto, Guid projetoId)
    {
        try
        {
            var projeto = await _unitOfWork.Projetos.GetByIdAsync(projetoId);
            if (projeto == null)
            {
                _logger.LogWarning("Projeto não encontrado: {ProjetoId}", projetoId);
                throw new NotFoundException("Projeto não encontrado");
            }

            if (tarefaDto.DataEntrega < DateTime.UtcNow.Date)
            {
                throw new BusinessException("Data de entrega não pode ser no passado");
            }

            var tarefa = _mapper.Map<TarefaProjeto>(tarefaDto);
            tarefa.ProjetoId = projetoId;
            tarefa.CriadoEm = DateTime.UtcNow;

            await _unitOfWork.Tarefas.AddAsync(tarefa);
            await _unitOfWork.CompleteAsync();

            _logger.LogInformation("Tarefa criada com sucesso: {TarefaId}", tarefa.Id);
            return _mapper.Map<TarefaConsultaDTO>(tarefa);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar tarefa");
            throw;
        }
    }

    public async Task<TarefaConsultaDTO> ObterPorIdAsync(Guid id)
    {
        try
        {
            var tarefa = await _unitOfWork.Tarefas.GetByIdAsync(id);
            if (tarefa == null)
            {
                _logger.LogWarning("Tarefa não encontrada: {TarefaId}", id);
                throw new NotFoundException("Tarefa não encontrada");
            }

            return _mapper.Map<TarefaConsultaDTO>(tarefa);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter tarefa por ID: {TarefaId}", id);
            throw;
        }
    }

    public async Task<IEnumerable<TarefaConsultaDTO>> ListarPorProjetoAsync(Guid projetoId)
    {
        try
        {
            var projeto = await _unitOfWork.Projetos.GetByIdAsync(projetoId);
            if (projeto == null)
            {
                _logger.LogWarning("Projeto não encontrado: {ProjetoId}", projetoId);
                throw new NotFoundException("Projeto não encontrado");
            }

            var tarefas = (await _unitOfWork.Tarefas.GetAllAsync())
                .Where(t => t.ProjetoId == projetoId)
                .OrderBy(t => t.DataEntrega)
                .ToList();

            return _mapper.Map<IEnumerable<TarefaConsultaDTO>>(tarefas);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar tarefas por projeto: {ProjetoId}", projetoId);
            throw;
        }
    }

    public async Task<IEnumerable<TarefaKanbanDTO>> ListarParaKanbanAsync(Guid projetoId)
    {
        try
        {
            var projeto = await _unitOfWork.Projetos.GetByIdAsync(projetoId);
            if (projeto == null)
            {
                _logger.LogWarning("Projeto não encontrado: {ProjetoId}", projetoId);
                throw new NotFoundException("Projeto não encontrado");
            }

            var tarefas = (await _unitOfWork.Tarefas.GetAllAsync())
                .Where(t => t.ProjetoId == projetoId)
                .OrderBy(t => t.DataEntrega)
                .ToList();

            return _mapper.Map<IEnumerable<TarefaKanbanDTO>>(tarefas);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar tarefas para Kanban: {ProjetoId}", projetoId);
            throw;
        }
    }

    public async Task<TarefaConsultaDTO> AtualizarTarefaAsync(Guid id, TarefaAtualizacaoDTO tarefaDto)
    {
        try
        {
            var tarefa = await _unitOfWork.Tarefas.GetByIdAsync(id);
            if (tarefa == null)
            {
                _logger.LogWarning("Tarefa não encontrada para atualização: {TarefaId}", id);
                throw new NotFoundException("Tarefa não encontrada");
            }

            if (tarefaDto.DataEntrega < DateTime.UtcNow.Date)
            {
                throw new BusinessException("Data de entrega não pode ser no passado");
            }

            _mapper.Map(tarefaDto, tarefa);
            _unitOfWork.Tarefas.UpdateAsync(tarefa);
            await _unitOfWork.CompleteAsync();

            _logger.LogInformation("Tarefa atualizada com sucesso: {TarefaId}", id);
            return _mapper.Map<TarefaConsultaDTO>(tarefa);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar tarefa: {TarefaId}", id);
            throw;
        }
    }

    public async Task<TarefaConsultaDTO> AtualizarStatusAsync(Guid id, StatusTarefa status)
    {
        try
        {
            var tarefa = await _unitOfWork.Tarefas.GetByIdAsync(id);
            if (tarefa == null)
            {
                _logger.LogWarning("Tarefa não encontrada para atualização de status: {TarefaId}", id);
                throw new NotFoundException("Tarefa não encontrada");
            }

            tarefa.Status = status;
            _unitOfWork.Tarefas.UpdateAsync(tarefa);
            await _unitOfWork.CompleteAsync();

            _logger.LogInformation("Status da tarefa atualizado com sucesso: {TarefaId}", id);
            return _mapper.Map<TarefaConsultaDTO>(tarefa);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar status da tarefa: {TarefaId}", id);
            throw;
        }
    }

    public async Task<bool> ExcluirTarefaAsync(Guid id)
    {
        try
        {
            var tarefa = await _unitOfWork.Tarefas.GetByIdAsync(id);
            if (tarefa == null)
            {
                _logger.LogWarning("Tarefa não encontrada para exclusão: {TarefaId}", id);
                throw new NotFoundException("Tarefa não encontrada");
            }

            await _unitOfWork.Tarefas.DeleteAsync(tarefa);
            await _unitOfWork.CompleteAsync();

            _logger.LogInformation("Tarefa excluída com sucesso: {TarefaId}", id);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao excluir tarefa: {TarefaId}", id);
            throw;
        }
    }
}