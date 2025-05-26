using DevInsight.Core.DTOs;
using DevInsight.Core.Exceptions;
using DevInsight.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevInsight.API.Controllers;

[Authorize]
[ApiController]
[Route("api/projetos")]
public class ProjetoController : ControllerBase
{
    private readonly IProjetoService _projetoService;
    private readonly ILogger<ProjetoController> _logger;

    public ProjetoController(IProjetoService projetoService, ILogger<ProjetoController> logger)
    {
        _projetoService = projetoService;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> CriarProjeto([FromBody] ProjetoCriacaoDTO projetoDto)
    {
        try
        {
            var usuarioId = GetUsuarioId();
            var projetoCriado = await _projetoService.CriarProjetoAsync(projetoDto, usuarioId);
            return CreatedAtAction(nameof(ObterPorId), new { id = projetoCriado.Id }, projetoCriado);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar projeto");
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ObterPorId(Guid id)
    {
        try
        {
            var projeto = await _projetoService.ObterPorIdAsync(id);
            return Ok(projeto);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter projeto por ID: {ProjetoId}", id);
            return StatusCode(500, new { message = "Ocorreu um erro interno" });
        }
    }

    [HttpGet]
    public async Task<IActionResult> ListarTodos()
    {
        try
        {
            var projetos = await _projetoService.ListarTodosAsync();
            return Ok(projetos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar projetos");
            return StatusCode(500, new { message = "Ocorreu um erro interno" });
        }
    }

    [HttpGet("usuario/{usuarioId}")]
    [Authorize(Roles = "Admin,Consultor")]
    public async Task<IActionResult> ListarPorUsuario(Guid usuarioId)
    {
        try
        {
            var projetos = await _projetoService.ListarPorUsuarioAsync(usuarioId);
            return Ok(projetos);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar projetos por usuário: {UsuarioId}", usuarioId);
            return StatusCode(500, new { message = "Ocorreu um erro interno" });
        }
    }

    [HttpGet("meus-projetos")]
    public async Task<IActionResult> ListarMeusProjetos()
    {
        try
        {
            var usuarioId = GetUsuarioId();
            var projetos = await _projetoService.ListarPorUsuarioAsync(usuarioId);
            return Ok(projetos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar projetos do usuário atual");
            return StatusCode(500, new { message = "Ocorreu um erro interno" });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> AtualizarProjeto(Guid id, [FromBody] ProjetoAtualizacaoDTO projetoDto)
    {
        try
        {
            var projetoAtualizado = await _projetoService.AtualizarProjetoAsync(id, projetoDto);
            return Ok(projetoAtualizado);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar projeto: {ProjetoId}", id);
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin,Consultor")]
    public async Task<IActionResult> ExcluirProjeto(Guid id)
    {
        try
        {
            var resultado = await _projetoService.ExcluirProjetoAsync(id);
            if (resultado)
                return NoContent();

            return BadRequest(new { message = "Não foi possível excluir o projeto" });
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao excluir projeto: {ProjetoId}", id);
            return StatusCode(500, new { message = "Ocorreu um erro interno" });
        }
    }

    private Guid GetUsuarioId()
    {
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "id");
        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var usuarioId))
        {
            throw new UnauthorizedAccessException("Usuário não autenticado corretamente");
        }
        return usuarioId;
    }
}