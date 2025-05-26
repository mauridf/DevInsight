using AutoMapper;
using DevInsight.Core.DTOs;
using DevInsight.Core.Entities;
using DevInsight.Core.Exceptions;
using DevInsight.Core.Interfaces;
using DevInsight.Core.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace DevInsight.Infrastructure.Services;

public class RequisitoService : IRequisitoService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<RequisitoService> _logger;

    public RequisitoService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<RequisitoService> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<RequisitoConsultaDTO> CriarRequisitoAsync(RequisitoCriacaoDTO requisitoDto, Guid projetoId)
    {
        try
        {
            var projeto = await _unitOfWork.Projetos.GetByIdAsync(projetoId);
            if (projeto == null)
            {
                _logger.LogWarning("Projeto não encontrado: {ProjetoId}", projetoId);
                throw new NotFoundException("Projeto não encontrado");
            }

            var requisito = _mapper.Map<Requisito>(requisitoDto);
            requisito.ProjetoId = projetoId;
            requisito.CriadoEm = DateTime.UtcNow;

            await _unitOfWork.Requisitos.AddAsync(requisito);
            await _unitOfWork.CompleteAsync();

            _logger.LogInformation("Requisito criado com sucesso: {RequisitoId}", requisito.Id);
            return _mapper.Map<RequisitoConsultaDTO>(requisito);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar requisito");
            throw;
        }
    }

    public async Task<RequisitoConsultaDTO> ObterPorIdAsync(Guid id)
    {
        try
        {
            var requisito = await _unitOfWork.Requisitos.GetByIdAsync(id);
            if (requisito == null)
            {
                _logger.LogWarning("Requisito não encontrado: {RequisitoId}", id);
                throw new NotFoundException("Requisito não encontrado");
            }

            return _mapper.Map<RequisitoConsultaDTO>(requisito);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter requisito por ID: {RequisitoId}", id);
            throw;
        }
    }

    public async Task<IEnumerable<RequisitoConsultaDTO>> ListarPorProjetoAsync(Guid projetoId)
    {
        try
        {
            var projeto = await _unitOfWork.Projetos.GetByIdAsync(projetoId);
            if (projeto == null)
            {
                _logger.LogWarning("Projeto não encontrado: {ProjetoId}", projetoId);
                throw new NotFoundException("Projeto não encontrado");
            }

            var requisitos = (await _unitOfWork.Requisitos.GetAllAsync())
                .Where(r => r.ProjetoId == projetoId)
                .ToList();

            return _mapper.Map<IEnumerable<RequisitoConsultaDTO>>(requisitos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar requisitos por projeto: {ProjetoId}", projetoId);
            throw;
        }
    }

    public async Task<RequisitoConsultaDTO> AtualizarRequisitoAsync(Guid id, RequisitoAtualizacaoDTO requisitoDto)
    {
        try
        {
            var requisito = await _unitOfWork.Requisitos.GetByIdAsync(id);
            if (requisito == null)
            {
                _logger.LogWarning("Requisito não encontrado para atualização: {RequisitoId}", id);
                throw new NotFoundException("Requisito não encontrado");
            }

            _mapper.Map(requisitoDto, requisito);
            await _unitOfWork.Requisitos.UpdateAsync(requisito);
            await _unitOfWork.CompleteAsync();

            _logger.LogInformation("Requisito atualizado com sucesso: {RequisitoId}", id);
            return _mapper.Map<RequisitoConsultaDTO>(requisito);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar requisito: {RequisitoId}", id);
            throw;
        }
    }

    public async Task<bool> ExcluirRequisitoAsync(Guid id)
    {
        try
        {
            var requisito = await _unitOfWork.Requisitos.GetByIdAsync(id);
            if (requisito == null)
            {
                _logger.LogWarning("Requisito não encontrado para exclusão: {RequisitoId}", id);
                throw new NotFoundException("Requisito não encontrado");
            }

            await _unitOfWork.Requisitos.DeleteAsync(requisito);
            await _unitOfWork.CompleteAsync();

            _logger.LogInformation("Requisito excluído com sucesso: {RequisitoId}", id);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao excluir requisito: {RequisitoId}", id);
            throw;
        }
    }
}