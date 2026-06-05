using System.Security.Claims;
using Core.Authorization;
using Core.Dto;
using Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace WebApi.Controllers;
[ApiController]
[Route("api/auth")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        try
        {
            var response = await authService.LoginAsync(dto);
            return Ok(response);
        }
        catch (Exception ex) when (ex.Message.Contains("Nieprawidłowy")
                                    || ex.Message.Contains("zablokowane")
                                    || ex.Message.Contains("nieaktywne"))
        {
            return Unauthorized(new { message = ex.Message });
        }
    }
    [HttpPost("refresh")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenDto dto)
    {
        var response = await authService.RefreshTokenAsync(dto);
        return Ok(response);
    }
    [HttpPost("revoke")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Revoke([FromBody] string refreshToken)
    {
        await authService.RevokeTokenAsync(refreshToken);
        return NoContent();
    }
    [HttpGet("me")]
    [Authorize]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    public IActionResult Me()
    {
        var user = new UserDto
        {
            Id = User.FindFirstValue(ClaimTypes.NameIdentifier)!,
            Email = User.FindFirstValue(ClaimTypes.Email)!,
            FirstName = User.FindFirstValue(ClaimTypes.GivenName)!,
            LastName = User.FindFirstValue(ClaimTypes.Surname)!,
            Department = User.FindFirstValue("department")!,
            FullName = User.FindFirstValue(ClaimTypes.Surname) + " " + User.FindFirstValue(ClaimTypes.GivenName),
            Roles = User.FindAll(ClaimTypes.Role).Select(c => c.Value)
        };
        return Ok(user);
    }
}

