using Microsoft.AspNetCore.Identity;

namespace Mec126.Models;

public class ApplicationUser : IdentityUser
{
	public string? DisplayName { get; set; }
}
