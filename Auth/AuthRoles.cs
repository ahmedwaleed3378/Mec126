namespace Mec126.Auth;

/// <summary>
/// Role names used in JWT claims and [Authorize(Roles = ...)] attributes.
/// </summary>
public static class AuthRoles
{
	public const string Admin = "Admin";
	public const string User = "User";

	/// <summary>Both roles can read products.</summary>
	public const string ReadProducts = $"{User},{Admin}";
}
