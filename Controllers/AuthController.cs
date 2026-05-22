using Mec126.Auth;
using Mec126.Auth.Services;
using Mec126.Common.Extensions;
using Mec126.Common.Responses;
using Mec126.Models;
using Mec126.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Mec126.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
	private readonly UserManager<ApplicationUser> _userManager;
	private readonly RoleManager<IdentityRole> _roleManager;
	private readonly TokenService _tokenService;
	private readonly JwtSettings _jwtSettings;

	public AuthController(
		UserManager<ApplicationUser> userManager,
		RoleManager<IdentityRole> roleManager,
		TokenService tokenService,
		IOptions<JwtSettings> jwtSettings)
	{
		_userManager = userManager;
		_roleManager = roleManager;
		_tokenService = tokenService;
		_jwtSettings = jwtSettings.Value;
	}

	/// <summary>
	/// Creates an account in AspNetUsers (hashed password). First account gets Admin; others get User.
	/// </summary>
	[HttpPost("register")]
	[AllowAnonymous]
	public async Task<ActionResult<ApiResponse<LoginResponse>>> Register([FromBody] RegisterRequest request)
	{
		if (!ModelState.IsValid)
			return this.ValidationFail<LoginResponse>();

		if (await _userManager.FindByNameAsync(request.Username) is not null)
			return Conflict(ApiResponse<LoginResponse>.Fail("Username is already taken."));

		if (await _userManager.FindByEmailAsync(request.Email) is not null)
			return Conflict(ApiResponse<LoginResponse>.Fail("Email is already registered."));

		var isFirstAccount = !await _userManager.Users.AnyAsync();

		var user = new ApplicationUser
		{
			UserName = request.Username,
			Email = request.Email,
			DisplayName = request.Username,
		};

		var createResult = await _userManager.CreateAsync(user, request.Password);
		if (!createResult.Succeeded)
		{
			var errors = createResult.Errors.Select(e => e.Description);
			return BadRequest(ApiResponse<LoginResponse>.Fail("Registration failed.", errors));
		}

		var role = isFirstAccount ? AuthRoles.Admin : AuthRoles.User;
		await EnsureRoleExistsAsync(role);
		await _userManager.AddToRoleAsync(user, role);

		return await BuildLoginResponse(user);
	}

	/// <summary>
	/// Validates credentials against AspNetUsers and returns a JWT.
	/// </summary>
	[HttpPost("login")]
	[AllowAnonymous]
	public async Task<ActionResult<ApiResponse<LoginResponse>>> Login([FromBody] LoginRequest request)
	{
		if (!ModelState.IsValid)
			return this.ValidationFail<LoginResponse>();

		var user = await _userManager.FindByNameAsync(request.Username);
		if (user is null || !await _userManager.CheckPasswordAsync(user, request.Password))
			return Unauthorized(ApiResponse<LoginResponse>.Fail("Invalid username or password."));

		return await BuildLoginResponse(user);
	}

	private async Task EnsureRoleExistsAsync(string roleName)
	{
		if (!await _roleManager.RoleExistsAsync(roleName))
			await _roleManager.CreateAsync(new IdentityRole(roleName));
	}

	private async Task<ActionResult<ApiResponse<LoginResponse>>> BuildLoginResponse(ApplicationUser user)
	{
		var roles = await _userManager.GetRolesAsync(user);
		var primaryRole = roles.FirstOrDefault() ?? AuthRoles.User;

		var token = _tokenService.CreateToken(
			user.Id,
			user.UserName ?? user.Email ?? "",
			roles);

		return Ok(ApiResponse<LoginResponse>.Ok(new LoginResponse
		{
			Token = token,
			Username = user.UserName ?? "",
			Role = primaryRole,
			ExpiresInMinutes = _jwtSettings.ExpiresInMinutes,
		}, "Login successful."));
	}
}
