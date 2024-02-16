using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Security.Api.Dtos;
using Security.Api.Options;

namespace Security.Api;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
	private readonly UserManager<IdentityUser> _userManager;
	private readonly IEmailSender _emailSender;
	private readonly RoleManager<IdentityRole> _roleManager;
	private readonly SignInManager<IdentityUser> _signInManager;
	private readonly JwtOption _jwtOption;

	public AuthController(UserManager<IdentityUser> userManager, IEmailSender emailSender, RoleManager<IdentityRole> roleManager, SignInManager<IdentityUser> signInManager, IOptions<JwtOption> jwtOption)
	{
		_userManager = userManager;
		_emailSender = emailSender;
		_roleManager = roleManager;
		_signInManager = signInManager;
		_jwtOption = jwtOption.Value;
	}

	[HttpPost("sign-up")]
	public async Task<IActionResult> SignUp([FromBody] SignUpDto signUpDto)
	{
		var isEmailUnique = await _userManager.FindByEmailAsync(signUpDto.Email) == null;
		if (isEmailUnique == false)
			ModelState.AddModelError("root", "Failed to register user. Please try again.");

		// validate is Role is valid
		var isRoleValid = await _roleManager.RoleExistsAsync(signUpDto.Role);
		if (isRoleValid == false)
			ModelState.AddModelError("Role", "Invalid role");

		if (ModelState.IsValid == false)
		{
			return BadRequest(ModelState.ToBadRequestResponse());
		}

		var user = new IdentityUser
		{
			UserName = signUpDto.Email,
			Email = signUpDto.Email
		};

		var userResult = await _userManager.CreateAsync(user, signUpDto.Password);
		if (userResult.Succeeded == false)
		{
			foreach (var error in userResult.Errors)
				ModelState.AddModelError("root", error.Description);

			return BadRequest(ModelState.ToBadRequestResponse());
		}

		user = await _userManager.FindByEmailAsync(signUpDto.Email);
		await _userManager.AddToRoleAsync(user!, signUpDto.Role);

		var token = await _userManager.GenerateEmailConfirmationTokenAsync(user!);
		var confirmationLink = Url.Action("ConfirmEmail", "Auth", new { userId = user!.Id, token }, Request.Scheme);
		await _emailSender.SendEmailAsync("Admin", user.Email!, "Confirm your email", confirmationLink!);

		return Ok(userResult);
	}

	// ConfirmEmail
	[HttpGet("confirm-email")]
	public async Task<IActionResult> ConfirmEmail(string userId, string token)
	{
		var user = await _userManager.FindByIdAsync(userId);
		if (user == null)
			return BadRequest("Invalid token");

		var result = await _userManager.ConfirmEmailAsync(user, token);
		if (result.Succeeded == false)
			return BadRequest("Invalid token");

		return Ok("Email confirmed");
	}

	[HttpPost("login")]
	public async Task<IActionResult> SignIn([FromBody] SignInDto signInDto)
	{
		if (ModelState.IsValid == false)
			return BadRequest(ModelState.ToBadRequestResponse());

		// sign in
		var signInResult = await _signInManager.PasswordSignInAsync(signInDto.Email, signInDto.Password, false, false);
		if (signInResult.Succeeded == false)
		{
			ModelState.AddModelError("root", "Invalid login");
			return BadRequest(ModelState.ToBadRequestResponse());
		}

		var user = await _userManager.FindByEmailAsync(signInDto.Email);
		var roles = await _userManager.GetRolesAsync(user!);


		List<Claim> claims =
		[
			new Claim(ClaimTypes.Name, signInDto.Email),
			new Claim(ClaimTypes.Role, roles[0]),
			new Claim("City", "Depok")
		];

		var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOption.Key));
		var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
		var token = new JwtSecurityToken(_jwtOption.Issuer, _jwtOption.Audience, claims, expires: DateTime.Now.AddMinutes(30), signingCredentials: creds);


		return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
	}

	// oauth facebook
	[HttpGet("facebook")]
	public IActionResult Facebook()
	{
		var properties = _signInManager.ConfigureExternalAuthenticationProperties("Facebook", Url.Action("FacebookCallback", "Auth"));
		return Challenge(properties, "Facebook");
	}

	// facebook callback
	[HttpGet("facebook-callback")]
	public async Task<IActionResult> FacebookCallback()
	{
		var info = await _signInManager.GetExternalLoginInfoAsync();
		if (info == null)
			return BadRequest("Failed to login with Facebook");

		var email = info.Principal.FindFirstValue(ClaimTypes.Email);

		var user = new IdentityUser
		{
			UserName = email,
			Email = email
		};

		var userResult = await _userManager.CreateAsync(user);

		return Ok("Facebook callback");
	}

}
