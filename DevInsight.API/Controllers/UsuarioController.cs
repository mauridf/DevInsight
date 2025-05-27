using DevInsight.Core.DTOs;
using DevInsight.Core.Exceptions;
using DevInsight.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevInsight.API.Controllers;

[Authorize]
[ApiController]
[Route("api/usuarios")]
public class UsuarioController : ControllerBase
{
    private readonly IUsuarioService _usuarioService;

    public UsuarioController(IUsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ObterPorId(Guid id)
    {
        try
        {
            var usuario = await _usuarioService.ObterPorIdAsync(id);
            return Ok(usuario);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Ocorreu um erro interno");
        }
    }

    [HttpGet("email/{email}")]
    public async Task<IActionResult> ObterPorEmail(string email)
    {
        try
        {
            var usuario = await _usuarioService.ObterPorEmailAsync(email);
            return Ok(usuario);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Ocorreu um erro interno");
        }
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ListarTodos()
    {
        try
        {
            var usuarios = await _usuarioService.ListarTodosAsync();
            return Ok(usuarios);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Ocorreu um erro interno");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Atualizar(Guid id, [FromBody] UsuarioAtualizacaoDTO atualizacaoDto)
    {
        try
        {
            var usuarioAtualizado = await _usuarioService.AtualizarAsync(id, atualizacaoDto);
            return Ok(usuarioAtualizado);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Ocorreu um erro interno");
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Excluir(Guid id)
    {
        try
        {
            var resultado = await _usuarioService.ExcluirAsync(id);
            if (resultado)
                return NoContent();

            return BadRequest("Não foi possível excluir o usuário");
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Ocorreu um erro interno");
        }
    }
}