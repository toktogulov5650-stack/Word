using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Word.API.Contracts;
using Word.API.Contracts.Auth;
using Word.Application.Abstractions.Services;


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


    [HttpPost("google")]
    public async Task<ActionResult<AuthResponse>> GoogleAsync(
        GoogleLoginRequest request,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.IdToken))
            return BadRequest("IdToken is required");

        var result = await _authService.SignInWithGoogleAsync(request.IdToken, cancellationToken);

        return Ok(new AuthResponse
        {
            AccessToken = result.AccessToken,
            User = new CurrentUserResponse
            {
                Id = result.User.Id,
                Email = result.User.Email,
                Name = result.User.Name,
            }
        });
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
}
