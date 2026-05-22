namespace Mec126.Models.DTO;

public class LoginResponse
{
	public string Token { get; set; } = string.Empty;
	public string Username { get; set; } = string.Empty;
	public string Role { get; set; } = string.Empty;
	public int ExpiresInMinutes { get; set; }
}
