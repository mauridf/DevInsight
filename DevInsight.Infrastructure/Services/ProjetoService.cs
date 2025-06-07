using AutoMapper;
using DevInsight.Core.DTOs;
using DevInsight.Core.Entities;
using DevInsight.Core.Enums;
using DevInsight.Core.Exceptions;
using DevInsight.Core.Interfaces;
using DevInsight.Core.Interfaces.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace DevInsight.Infrastructure.Services;

public class ProjetoService : IProjetoService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<ProjetoService> _logger;

    public ProjetoService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<ProjetoService> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ProjetoConsultaDTO> CriarProjetoAsync(ProjetoCriacaoDTO projetoDto, Guid usuarioId)
    {
        try
        {
            var usuario = await _unitOfWork.Usuarios.GetByIdAsync(usuarioId);
            if (usuario == null)
            {
                _logger.LogWarning("Usuário não encontrado: {UsuarioId}", usuarioId);
                throw new NotFoundException("Usuário não encontrado");
            }

            // Primeiro mapeia o DTO para a entidade
            var projeto = _mapper.Map<ProjetoConsultoria>(projetoDto);

            // Depois define as propriedades que precisam de lógica especial
            projeto.CriadoPorId = usuarioId;
            projeto.CriadoPor = usuario;
            projeto.CriadoEm = DateTime.UtcNow;

            await _unitOfWork.Projetos.AddAsync(projeto);
            await _unitOfWork.CompleteAsync();

            _logger.LogInformation("Projeto criado com sucesso: {ProjetoId}", projeto.Id);
            return _mapper.Map<ProjetoConsultaDTO>(projeto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar projeto");
            throw;
        }
    }

    public async Task<ProjetoDetalhesDTO> ObterPorIdAsync(Guid id)
    {
        try
        {
            var projeto = await _unitOfWork.Projetos.GetByIdAsync(id);
            if (projeto == null)
            {
                _logger.LogWarning("Projeto não encontrado: {ProjetoId}", id);
                throw new NotFoundException("Projeto não encontrado");
            }

            // O mapeamento agora é completo pelo AutoMapper
            return _mapper.Map<ProjetoDetalhesDTO>(projeto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter projeto por ID: {ProjetoId}", id);
            throw;
        }
    }

    public async Task<IEnumerable<ProjetoConsultaDTO>> ListarTodosAsync()
    {
        try
        {
            // Carrega os projetos incluindo os dados do CriadoPor em uma única consulta
            var projetos = await _unitOfWork.Projetos.GetAllAsync(include: q => q.Include(p => p.CriadoPor));
            return _mapper.Map<IEnumerable<ProjetoConsultaDTO>>(projetos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar todos os projetos");
            throw;
        }
    }

    public async Task<ProjetoConsultaDTO> AtualizarProjetoAsync(Guid id, ProjetoAtualizacaoDTO projetoDto)
    {
        try
        {
            var projeto = await _unitOfWork.Projetos.GetByIdAsync(id);
            if (projeto == null)
            {
                _logger.LogWarning("Projeto não encontrado para atualização: {ProjetoId}", id);
                throw new NotFoundException("Projeto não encontrado");
            }

            _mapper.Map(projetoDto, projeto);
            await _unitOfWork.Projetos.UpdateAsync(projeto);
            await _unitOfWork.CompleteAsync();

            _logger.LogInformation("Projeto atualizado com sucesso: {ProjetoId}", id);
            return _mapper.Map<ProjetoConsultaDTO>(projeto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar projeto: {ProjetoId}", id);
            throw;
        }
    }

    public async Task<bool> ExcluirProjetoAsync(Guid id)
    {
        try
        {
            var projeto = await _unitOfWork.Projetos.GetByIdAsync(id);
            if (projeto == null)
            {
                _logger.LogWarning("Projeto não encontrado para exclusão: {ProjetoId}", id);
                throw new NotFoundException("Projeto não encontrado");
            }

            await _unitOfWork.Projetos.DeleteAsync(projeto);
            await _unitOfWork.CompleteAsync();

            _logger.LogInformation("Projeto excluído com sucesso: {ProjetoId}", id);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao excluir projeto: {ProjetoId}", id);
            throw;
        }
    }

    public async Task<IEnumerable<ProjetoConsultaDTO>> ListarPorUsuarioAsync(Guid usuarioId)
    {
        try
        {
            var usuario = await _unitOfWork.Usuarios.GetByIdAsync(usuarioId);
            if (usuario == null)
            {
                _logger.LogWarning("Usuário não encontrado: {UsuarioId}", usuarioId);
                throw new NotFoundException("Usuário não encontrado");
            }

            // Carrega os projetos incluindo os dados do CriadoPor em uma única consulta
            var projetos = await _unitOfWork.Projetos.GetAllAsync(
                filter: p => p.CriadoPorId == usuarioId,
                include: q => q.Include(p => p.CriadoPor));

            return _mapper.Map<IEnumerable<ProjetoConsultaDTO>>(projetos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar projetos por usuário: {UsuarioId}", usuarioId);
            throw;
        }
    }

    public async Task<DashboardDTO> ObterDadosDashboardAsync(Guid usuarioId)
    {
        try
        {
            // Verifica se o usuário existe
            var usuario = await _unitOfWork.Usuarios.GetByIdAsync(usuarioId);
            if (usuario == null)
            {
                _logger.LogWarning("Usuário não encontrado: {UsuarioId}", usuarioId);
                throw new NotFoundException("Usuário não encontrado");
            }

            // Obtém todos os projetos do usuário
            var projetos = await _unitOfWork.Projetos.GetByUsuarioIdAsync(usuarioId);

            // Conta os projetos por status (agora usando Count() como método)
            return new DashboardDTO
            {
                TotalProjetos = projetos.Count(),
                ProjetosEmAndamento = projetos.Count(p => p.Status == StatusProjeto.EmAndamento),
                ProjetosFinalizados = projetos.Count(p => p.Status == StatusProjeto.Finalizado)
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter dados do dashboard para o usuário: {UsuarioId}", usuarioId);
            throw;
        }
    }

    public async Task<ProjetoConsultoria> ObterProjetoCompleto(Guid projetoId)
    {
        try
        {
            var projeto = await _unitOfWork.Projetos
                .Query()
                .Include(p => p.CriadoPor)
                .Include(p => p.PersonasChaves)
                .Include(p => p.FasesProjeto)
                .Include(p => p.EstimativaCustos)
                .Include(p => p.StakeHolders)
                .Include(p => p.Funcionalidades)
                .Include(p => p.Requisitos)
                .Include(p => p.Documentos)
                .Include(p => p.Reunioes)
                .Include(p => p.Tarefas)
                .Include(p => p.ValidacoesTecnicas)
                .Include(p => p.Entregas)
                .Include(p => p.Solucoes)
                .Include(p => p.Entregaveis)
                .FirstOrDefaultAsync(p => p.Id == projetoId);

            if (projeto == null)
            {
                _logger.LogWarning("Projeto não encontrado: {ProjetoId}", projetoId);
                throw new NotFoundException("Projeto não encontrado");
            }

            _logger.LogInformation("Projeto completo carregado: {ProjetoId}", projetoId);
            return projeto;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter projeto completo: {ProjetoId}", projetoId);
            throw;
        }
    }

    public async Task<IEnumerable<ValidacaoTecnica>> ObterValidacoesTecnicas(Guid projetoId)
    {
        try
        {
            var validacoes = await _unitOfWork.ValidacoesTecnicas
                .GetAllAsync(v => v.ProjetoId == projetoId);

            _logger.LogDebug("Validações técnicas encontradas: {Count}", validacoes.Count());
            return validacoes;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter validações técnicas para o projeto: {ProjetoId}", projetoId);
            throw;
        }
    }

    public async Task<IEnumerable<EntregaFinal>> ObterEntregasFinais(Guid projetoId)
    {
        try
        {
            var entregas = await _unitOfWork.Entregas
                .GetAllAsync(e => e.ProjetoId == projetoId);

            _logger.LogDebug("Entregas finais encontradas: {Count}", entregas.Count());
            return entregas;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter entregas finais para o projeto: {ProjetoId}", projetoId);
            throw;
        }
    }

    public async Task<IEnumerable<TarefaProjeto>> ObterTarefasKanban(Guid projetoId)
    {
        try
        {
            var tarefas = await _unitOfWork.Tarefas
                .GetAllAsync(t => t.ProjetoId == projetoId);

            _logger.LogDebug("Tarefas encontradas: {Count}", tarefas.Count());
            return tarefas;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter tarefas para o projeto: {ProjetoId}", projetoId);
            throw;
        }
    }

    public async Task<SolucaoProposta> ObterSolucaoProposta(Guid projetoId)
    {
        try
        {
            var solucao = await _unitOfWork.Solucoes
                .FirstOrDefaultAsync(s => s.ProjetoId == projetoId);

            if (solucao == null)
            {
                _logger.LogWarning("Solução proposta não encontrada para o projeto: {ProjetoId}", projetoId);
                throw new NotFoundException("Solução proposta não encontrada");
            }

            _logger.LogDebug("Solução proposta encontrada: {SolucaoId}", solucao.Id);
            return solucao;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter solução proposta para o projeto: {ProjetoId}", projetoId);
            throw;
        }
    }
}