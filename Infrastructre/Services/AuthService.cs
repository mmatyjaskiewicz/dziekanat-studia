using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Core.Dto;
using Core.Security;
using Core.Services;
using Infrastructre.EntityFramework.Context;
using Infrastructre.EntityFramework.Entities;
using Infrastructre.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
namespace Infrastructre.Services;
public class AuthService : IAuthService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly UniversityIdentityDbContext _context;
    private readonly JwtSettings _jwtOptions;
    public AuthService(UserManager<AppUser> userManager, UniversityIdentityDbContext context, JwtSettings jwtOptions)
    {
        _userManager = userManager;
        _context = context;
        _jwtOptions = jwtOptions;
    }
    public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
    {
        var user = _userManager.Users
                       .FirstOrDefault(u => u.Email == dto.Email)
                   ?? throw new Exception("Nieprawidłowy email lub hasło.");
        if (!await _userManager.CheckPasswordAsync(user, dto.Password))
        {
            await _userManager.AccessFailedAsync(user);
            throw new Exception("Nieprawidłowy email lub hasło.");
        }
        if (user.Status != SystemUserStatus.Active)
            throw new Exception("Konto jest nieaktywne.");
        if (await _userManager.IsLockedOutAsync(user))
            throw new Exception("Konto jest zablokowane.");
        await _userManager.ResetAccessFailedCountAsync(user);
        return await GenerateAuthResponseAsync(user);
    }
    public async Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenDto dto)
    {
        var principal = GetPrincipalFromExpiredToken(dto.AccessToken);
        var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new Exception("Nieprawidłowy token.");
        var user = await _userManager.FindByIdAsync(userId)
            ?? throw new Exception("UÅ¼ytkownik nie istnieje.");
        var refreshToken = await _context.RefreshTokens
            .FirstOrDefaultAsync(t =>
                t.Token == dto.RefreshToken &&
                t.UserId == userId)
            ?? throw new Exception("Nieprawidłowy refresh token.");
        if (!refreshToken.IsActive)
            throw new Exception("Refresh token wygasł lub został odwołany.");
        var newResponse = await GenerateAuthResponseAsync(user);
        refreshToken.Revoke(newResponse.RefreshToken);
        await _context.SaveChangesAsync();
        return newResponse;
    }
    public async Task RevokeTokenAsync(string refreshToken)
    {
        var token = await _context.RefreshTokens
            .FirstOrDefaultAsync(t => t.Token == refreshToken)
            ?? throw new Exception("Nie istnieje " + refreshToken);
        if (!token.IsActive)
            throw new Exception("Token jest juÅ¼ nieaktywny.");
        token.Revoke();
        await _context.SaveChangesAsync();
    }
    private async Task<AuthResponseDto> GenerateAuthResponseAsync(AppUser user)
    {
        Console.WriteLine("STEP 1");
        var roles = await _userManager.GetRolesAsync(user);
        Console.WriteLine("STEP 2");
        var accessToken = GenerateAccessToken(user, roles);
        Console.WriteLine("STEP 3");
        var refreshToken = await GenerateRefreshTokenAsync(user.Id);
        Console.WriteLine("STEP 4");
        return new AuthResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken.Token,
            ExpiresAt = DateTime.UtcNow.AddMinutes(_jwtOptions.ExpirationInMinutes),
            User = new UserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email!,
                FullName = user.FullName,
                Department = user.Department,
                Status = user.Status,
                Roles = roles
            }
        };
    }
    private string GenerateAccessToken(AppUser user, IList<string> roles)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Email, user.Email!),
            new(ClaimTypes.GivenName, user.FirstName),
            new(ClaimTypes.Surname, user.LastName),
            new("department", user.Department),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Iat,
                DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString())
        };
        claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));
        var credentials = new SigningCredentials(
            _jwtOptions.GetSymmetricKey(),
            SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddMinutes(_jwtOptions.ExpirationInMinutes),
            signingCredentials: credentials);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    private async Task<RefreshToken> GenerateRefreshTokenAsync(string userId)
    {
        var activeTokens = await _context.RefreshTokens
            .Where(t => t.UserId == userId && t.RevokedAt == null)
            .ToListAsync();
        foreach (var token in activeTokens)
            token.Revoke();
        var refreshToken = new RefreshToken
        {
            UserId = userId,
            Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
            ExpiresAt = DateTime.UtcNow.AddDays(_jwtOptions.RefreshTokenDays)
        };
        await _context.RefreshTokens.AddAsync(refreshToken);
        await _context.SaveChangesAsync();
        return refreshToken;
    }
    private ClaimsPrincipal GetPrincipalFromExpiredToken(string accessToken)
    {
        var parameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true,
            ValidIssuer = _jwtOptions.Issuer,
            ValidAudience = _jwtOptions.Audience,
            IssuerSigningKey = _jwtOptions.GetSymmetricKey()
        };
        var handler = new JwtSecurityTokenHandler();
        var principal = handler.ValidateToken(accessToken, parameters, out var securityToken);
        if (securityToken is not JwtSecurityToken jwtToken ||
            !jwtToken.Header.Alg.Equals(
                SecurityAlgorithms.HmacSha256,
                StringComparison.OrdinalIgnoreCase))
        {
            throw new Exception("Nieprawidłowy token.");
        }
        return principal;
    }
}

