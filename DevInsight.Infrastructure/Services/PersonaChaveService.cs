using AutoMapper;
using DevInsight.Core.DTOs;
using DevInsight.Core.Entities;
using DevInsight.Core.Exceptions;
using DevInsight.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace DevInsight.Infrastructure.Services;

public class PersonaChaveService : IPersonaChave
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<PersonasChave> _logger;

    public PersonaChaveService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<PersonasChave> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<PersonaChaveConsultaDTO> AtualizarPersonaAsync(Guid id, PersonaChaveAtualizacaoDTO personaDto)
    {
        try
        {
            var persona = await _unitOfWork.PersonasChaves.GetByIdAsync(id);
            if (persona == null)
            {
                _logger.LogWarning("Persona Chave não encontrada para atualização: {PersonaId}", id);
                throw new NotFoundException("Persona não encontrada");
            }

            _mapper.Map(personaDto, persona);
            await _unitOfWork.PersonasChaves.UpdateAsync(persona);
            await _unitOfWork.CompleteAsync();

            _logger.LogInformation("Funcionalidade atualizada com sucesso: {PersonaId}", id);
            return _mapper.Map<PersonaChaveConsultaDTO>(persona);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar persona: {PersonaId}", id);
            throw;
        }
    }

    public async Task<PersonaChaveConsultaDTO> CriarPersonaAsync(PersonaChaveCriacaoDTO personaDto, Guid projetoId)
    {
        try
        {
            var projeto = await _unitOfWork.Projetos.GetByIdAsync(projetoId);
            if (projeto == null)
            {
                _logger.LogWarning("Projeto não encontrado: {ProjetoId}", projetoId);
                throw new NotFoundException("Projeto não encontrado");
            }

            var persona = _mapper.Map<PersonasChave>(personaDto);
            persona.ProjetoId = projetoId;
            persona.CriadoEm = DateTime.UtcNow;

            await _unitOfWork.PersonasChaves.AddAsync(persona);
            await _unitOfWork.CompleteAsync();

            _logger.LogInformation("Persona Chave criada com sucesso: {PersonaId}", persona.Id);
            return _mapper.Map<PersonaChaveConsultaDTO>(persona);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar persona chave");
            throw;
        }
    }

    public async Task<bool> ExcluirPersonaAsync(Guid id)
    {
        try
        {
            var persona = await _unitOfWork.PersonasChaves.GetByIdAsync(id);
            if (persona == null)
            {
                _logger.LogWarning("Persona Chave não encontrada para exclusão: {PersonaId}", id);
                throw new NotFoundException("Persona Chave não encontrada");
            }

            await _unitOfWork.PersonasChaves.DeleteAsync(persona);
            await _unitOfWork.CompleteAsync();

            _logger.LogInformation("Persona Chave excluída com sucesso: {PersonaId}", id);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao excluir persona chave: {PersonaId}", id);
            throw;
        }
    }

    public async Task<IEnumerable<PersonaChaveConsultaDTO>> ListarPorProjetoAsync(Guid projetoId)
    {
        try
        {
            var projeto = await _unitOfWork.Projetos.GetByIdAsync(projetoId);
            if (projeto == null)
            {
                _logger.LogWarning("Projeto não encontrado: {ProjetoId}", projetoId);
                throw new NotFoundException("Projeto não encontrado");
            }

            var personas = (await _unitOfWork.PersonasChaves.GetAllAsync())
                .Where(p => p.ProjetoId == projetoId)
                .ToList();

            return _mapper.Map<IEnumerable<PersonaChaveConsultaDTO>>(personas);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar personas chaves por projeto: {ProjetoId}", projetoId);
            throw;
        }
    }

    public async Task<PersonaChaveConsultaDTO> ObterPorIdAsync(Guid id)
    {
        try
        {
            var persona = await _unitOfWork.PersonasChaves.GetByIdAsync(id);
            if (persona == null)
            {
                _logger.LogWarning("Persona Chave não encontrada: {PersonaId}", id);
                throw new NotFoundException("Persona Chave não encontrada");
            }

            return _mapper.Map<PersonaChaveConsultaDTO>(persona);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter persona chave por ID: {PersonaId}", id);
            throw;
        }
    }
}
