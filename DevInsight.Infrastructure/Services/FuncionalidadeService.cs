using AutoMapper;
using DevInsight.Core.DTOs;
using DevInsight.Core.Entities;
using DevInsight.Core.Exceptions;
using DevInsight.Core.Interfaces;
using DevInsight.Core.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace DevInsight.Infrastructure.Services;

public class FuncionalidadeService : IFuncionalidadeService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<FuncionalidadeService> _logger;

    public FuncionalidadeService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<FuncionalidadeService> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<FuncionalidadeConsultaDTO> CriarFuncionalidadeAsync(FuncionalidadeCriacaoDTO funcionalidadeDto, Guid projetoId)
    {
        try
        {
            var projeto = await _unitOfWork.Projetos.GetByIdAsync(projetoId);
            if (projeto == null)
            {
                _logger.LogWarning("Projeto não encontrado: {ProjetoId}", projetoId);
                throw new NotFoundException("Projeto não encontrado");
            }

            var funcionalidade = _mapper.Map<FuncionalidadeDesejada>(funcionalidadeDto);
            funcionalidade.ProjetoId = projetoId;
            funcionalidade.CriadoEm = DateTime.UtcNow;

            await _unitOfWork.Funcionalidades.AddAsync(funcionalidade);
            await _unitOfWork.CompleteAsync();

            _logger.LogInformation("Funcionalidade criada com sucesso: {FuncionalidadeId}", funcionalidade.Id);
            return _mapper.Map<FuncionalidadeConsultaDTO>(funcionalidade);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar funcionalidade");
            throw;
        }
    }

    public async Task<FuncionalidadeConsultaDTO> ObterPorIdAsync(Guid id)
    {
        try
        {
            var funcionalidade = await _unitOfWork.Funcionalidades.GetByIdAsync(id);
            if (funcionalidade == null)
            {
                _logger.LogWarning("Funcionalidade não encontrada: {FuncionalidadeId}", id);
                throw new NotFoundException("Funcionalidade não encontrada");
            }

            return _mapper.Map<FuncionalidadeConsultaDTO>(funcionalidade);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter funcionalidade por ID: {FuncionalidadeId}", id);
            throw;
        }
    }

    public async Task<IEnumerable<FuncionalidadeConsultaDTO>> ListarPorProjetoAsync(Guid projetoId)
    {
        try
        {
            var projeto = await _unitOfWork.Projetos.GetByIdAsync(projetoId);
            if (projeto == null)
            {
                _logger.LogWarning("Projeto não encontrado: {ProjetoId}", projetoId);
                throw new NotFoundException("Projeto não encontrado");
            }

            var funcionalidades = (await _unitOfWork.Funcionalidades.GetAllAsync())
                .Where(f => f.ProjetoId == projetoId)
                .ToList();

            return _mapper.Map<IEnumerable<FuncionalidadeConsultaDTO>>(funcionalidades);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar funcionalidades por projeto: {ProjetoId}", projetoId);
            throw;
        }
    }

    public async Task<FuncionalidadeConsultaDTO> AtualizarFuncionalidadeAsync(Guid id, FuncionalidadeAtualizacaoDTO funcionalidadeDto)
    {
        try
        {
            var funcionalidade = await _unitOfWork.Funcionalidades.GetByIdAsync(id);
            if (funcionalidade == null)
            {
                _logger.LogWarning("Funcionalidade não encontrada para atualização: {FuncionalidadeId}", id);
                throw new NotFoundException("Funcionalidade não encontrada");
            }

            _mapper.Map(funcionalidadeDto, funcionalidade);
            await _unitOfWork.Funcionalidades.UpdateAsync(funcionalidade);
            await _unitOfWork.CompleteAsync();

            _logger.LogInformation("Funcionalidade atualizada com sucesso: {FuncionalidadeId}", id);
            return _mapper.Map<FuncionalidadeConsultaDTO>(funcionalidade);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar funcionalidade: {FuncionalidadeId}", id);
            throw;
        }
    }

    public async Task<bool> ExcluirFuncionalidadeAsync(Guid id)
    {
        try
        {
            var funcionalidade = await _unitOfWork.Funcionalidades.GetByIdAsync(id);
            if (funcionalidade == null)
            {
                _logger.LogWarning("Funcionalidade não encontrada para exclusão: {FuncionalidadeId}", id);
                throw new NotFoundException("Funcionalidade não encontrada");
            }

            await _unitOfWork.Funcionalidades.DeleteAsync(funcionalidade);
            await _unitOfWork.CompleteAsync();

            _logger.LogInformation("Funcionalidade excluída com sucesso: {FuncionalidadeId}", id);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao excluir funcionalidade: {FuncionalidadeId}", id);
            throw;
        }
    }
}