using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AccountHub.Application.DTOs;
using AccountHub.Domain.Entities;
using AccountHub.Domain.Exceptions;
using AccountHub.Domain.Models;
using AccountHub.Domain.Options;
using AccountHub.Domain.Services;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace AccountHub.Infrastructure.Services;

public class JwtService : IJwtService
{
    private readonly JwtOptions _options;

    public JwtService(IOptions<JwtOptions> options)
    {
        _options = options.Value;
    }

    public string GenerateJwtAccessToken(UserEntity user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var claims = GenerateClaims(user);
        var jwtDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(_options.ExpiryMinutes),
            Issuer = _options.Issuer,
            Audience = _options.Audience,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey)), SecurityAlgorithms.HmacSha256)
        };
        var jwt = tokenHandler.WriteToken(tokenHandler.CreateToken(jwtDescriptor));
        return jwt;
    }

    private ClaimsIdentity GenerateClaims(UserEntity model)
    {
        var claims = new ClaimsIdentity();
        claims.AddClaim(new Claim(JwtRegisteredClaimNames.Email, model.Email!));
        claims.AddClaim(new Claim(ClaimTypes.Name, model.UserName!));
        claims.AddClaim(new Claim(JwtRegisteredClaimNames.NameId, model.Id));
        return claims;
    }

    public async Task<bool> ValidateJwtToken(string jwtToken)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_options.SecretKey);
        var tokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = true,
            ValidateLifetime = true
        };
        return (await tokenHandler.ValidateTokenAsync(jwtToken, tokenValidationParameters)).IsValid;
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    public async Task<string> GetUsernameFromExpiredToken(string jwtToken)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_options.SecretKey);
        var tokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = true,
            ValidateLifetime = false
        };
        var claims = await tokenHandler.ValidateTokenAsync(jwtToken, tokenValidationParameters);
        if(claims.IsValid && claims.ClaimsIdentity.Name is not null)
            return claims.ClaimsIdentity.Name;
        throw new InvalidTokenException("Invalid token");
    }
}