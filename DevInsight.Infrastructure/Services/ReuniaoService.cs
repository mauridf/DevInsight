using AutoMapper;
using DevInsight.Core.DTOs;
using DevInsight.Core.Entities;
using DevInsight.Core.Exceptions;
using DevInsight.Core.Interfaces;
using DevInsight.Core.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace DevInsight.Infrastructure.Services;

public class ReuniaoService : IReuniaoService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<ReuniaoService> _logger;

    public ReuniaoService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<ReuniaoService> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ReuniaoConsultaDTO> CriarReuniaoAsync(ReuniaoCriacaoDTO reuniaoDto, Guid projetoId)
    {
        try
        {
            var projeto = await _unitOfWork.Projetos.GetByIdAsync(projetoId);
            if (projeto == null)
            {
                _logger.LogWarning("Projeto não encontrado: {ProjetoId}", projetoId);
                throw new NotFoundException("Projeto não encontrado");
            }

            if (reuniaoDto.DataHora < DateTime.UtcNow)
            {
                throw new BusinessException("A data/hora da reunião não pode ser no passado");
            }

            var reuniao = _mapper.Map<Reuniao>(reuniaoDto);
            reuniao.ProjetoId = projetoId;
            reuniao.CriadoEm = DateTime.UtcNow;

            await _unitOfWork.Reunioes.AddAsync(reuniao);
            await _unitOfWork.CompleteAsync();

            _logger.LogInformation("Reunião criada com sucesso: {ReuniaoId}", reuniao.Id);
            return _mapper.Map<ReuniaoConsultaDTO>(reuniao);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar reunião");
            throw;
        }
    }

    public async Task<ReuniaoConsultaDTO> ObterPorIdAsync(Guid id)
    {
        try
        {
            var reuniao = await _unitOfWork.Reunioes.GetByIdAsync(id);
            if (reuniao == null)
            {
                _logger.LogWarning("Reunião não encontrada: {ReuniaoId}", id);
                throw new NotFoundException("Reunião não encontrada");
            }

            return _mapper.Map<ReuniaoConsultaDTO>(reuniao);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter reunião por ID: {ReuniaoId}", id);
            throw;
        }
    }

    public async Task<IEnumerable<ReuniaoConsultaDTO>> ListarPorProjetoAsync(Guid projetoId)
    {
        try
        {
            var projeto = await _unitOfWork.Projetos.GetByIdAsync(projetoId);
            if (projeto == null)
            {
                _logger.LogWarning("Projeto não encontrado: {ProjetoId}", projetoId);
                throw new NotFoundException("Projeto não encontrado");
            }

            var reunioes = (await _unitOfWork.Reunioes.GetAllAsync())
                .Where(r => r.ProjetoId == projetoId)
                .OrderBy(r => r.DataHora)
                .ToList();

            return _mapper.Map<IEnumerable<ReuniaoConsultaDTO>>(reunioes);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar reuniões por projeto: {ProjetoId}", projetoId);
            throw;
        }
    }

    public async Task<IEnumerable<ReuniaoConsultaDTO>> ListarProximasReunioesAsync(int dias = 7)
    {
        try
        {
            var dataInicio = DateTime.UtcNow;
            var dataFim = dataInicio.AddDays(dias);

            var reunioes = (await _unitOfWork.Reunioes.GetAllAsync())
                .Where(r => r.DataHora >= dataInicio && r.DataHora <= dataFim)
                .OrderBy(r => r.DataHora)
                .ToList();

            return _mapper.Map<IEnumerable<ReuniaoConsultaDTO>>(reunioes);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar próximas reuniões");
            throw;
        }
    }

    public async Task<ReuniaoConsultaDTO> AtualizarReuniaoAsync(Guid id, ReuniaoAtualizacaoDTO reuniaoDto)
    {
        try
        {
            var reuniao = await _unitOfWork.Reunioes.GetByIdAsync(id);
            if (reuniao == null)
            {
                _logger.LogWarning("Reunião não encontrada para atualização: {ReuniaoId}", id);
                throw new NotFoundException("Reunião não encontrada");
            }

            if (reuniaoDto.DataHora < DateTime.UtcNow)
            {
                throw new BusinessException("A data/hora da reunião não pode ser no passado");
            }

            _mapper.Map(reuniaoDto, reuniao);
            await _unitOfWork.Reunioes.UpdateAsync(reuniao);
            await _unitOfWork.CompleteAsync();

            _logger.LogInformation("Reunião atualizada com sucesso: {ReuniaoId}", id);
            return _mapper.Map<ReuniaoConsultaDTO>(reuniao);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar reunião: {ReuniaoId}", id);
            throw;
        }
    }

    public async Task<bool> ExcluirReuniaoAsync(Guid id)
    {
        try
        {
            var reuniao = await _unitOfWork.Reunioes.GetByIdAsync(id);
            if (reuniao == null)
            {
                _logger.LogWarning("Reunião não encontrada para exclusão: {ReuniaoId}", id);
                throw new NotFoundException("Reunião não encontrada");
            }

            await _unitOfWork.Reunioes.DeleteAsync(reuniao);
            await _unitOfWork.CompleteAsync();

            _logger.LogInformation("Reunião excluída com sucesso: {ReuniaoId}", id);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao excluir reunião: {ReuniaoId}", id);
            throw;
        }
    }
}