using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Word.API.Contracts.Auth;
using Word.Application.Abstractions.Services;
using Word.Application.DTOs.Auth;

namespace Word.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> RegisterAsync(
        RegisterRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await _authService.RegisterAsync(
            new RegisterRequestDto
            {
                Name = request.Name,
                Email = request.Email,
                Password = request.Password
            },
            cancellationToken);

        return Ok(MapAuthResponse(result));
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> LoginAsync(
        LoginRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await _authService.LoginAsync(
            new LoginRequestDto
            {
                Email = request.Email,
                Password = request.Password
            },
            cancellationToken);

        return Ok(MapAuthResponse(result));
    }

    [HttpPost("google")]
    public async Task<ActionResult<AuthResponse>> GoogleAsync(
        GoogleLoginRequest request,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.IdToken))
            return BadRequest("IdToken is required");

        var result = await _authService.SignInWithGoogleAsync(request.IdToken, cancellationToken);

        return Ok(MapAuthResponse(result));
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<ActionResult<CurrentUserResponse>> MeAsync(
        CancellationToken cancellationToken = default)
    {
        var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!int.TryParse(userIdValue, out var userId))
            return Unauthorized();

        var user = await _authService.GetCurrentUserAsync(userId, cancellationToken);

        if (user is null)
            return NotFound();

        return Ok(new CurrentUserResponse
        {
            Id = user.Id,
            Email = user.Email,
            Name = user.Name,
        });
    }

    private static AuthResponse MapAuthResponse(AuthResponseDto result)
    {
        return new AuthResponse
        {
            AccessToken = result.AccessToken,
            User = new CurrentUserResponse
            {
                Id = result.User.Id,
                Email = result.User.Email,
                Name = result.User.Name
            }
        };
    }
}
