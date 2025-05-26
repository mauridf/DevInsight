using DevInsight.Core.DTOs;
using DevInsight.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DevInsight.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("registrar")]
    public async Task<IActionResult> Registrar(UsuarioRegistroDto registroDto)
    {
        try
        {
            var resultado = await _authService.Registrar(registroDto);
            return Ok(resultado);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        try
        {
            var resultado = await _authService.Login(loginDto);
            return Ok(resultado);
        }
        catch (Exception ex)
        {
            return Unauthorized(ex.Message);
        }
    }
}