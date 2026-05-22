using System.ComponentModel.DataAnnotations;

namespace Mec126.Models.DTO;

// Credintials => Creds
public class LoginRequest
{
	[Required]
	public string Username { get; set; } = string.Empty;

	[Required]
	public string Password { get; set; } = string.Empty;
}
