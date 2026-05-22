using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Mec126.Auth.Services;

public class TokenService
{
	private readonly JwtSettings _settings;

	public TokenService(IOptions<JwtSettings> settings)
	{
		_settings = settings.Value;
	}

	public string CreateToken(string userId, string username, IEnumerable<string> roles)
	{
		var claims = new List<Claim>
		{
			new(ClaimTypes.NameIdentifier, userId),
			new(ClaimTypes.Name, username),
		};

		claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

		var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Key));
		var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

		var token = new JwtSecurityToken(
			issuer: _settings.Issuer,
			audience: _settings.Audience,
			claims: claims,
			expires: DateTime.UtcNow.AddMinutes(_settings.ExpiresInMinutes),
			signingCredentials: credentials);

		return new JwtSecurityTokenHandler().WriteToken(token);
	}
}
