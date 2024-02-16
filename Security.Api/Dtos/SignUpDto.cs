using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Security.Api.Enums;

namespace Security.Api;

public class SignUpDto
{
	[Required]
	[DefaultValue("farhanbantulm1@gmail.com")]
	public string Email { get; set; } = string.Empty;

	[Required]
	[DefaultValue("indonesia123B$")]
	public string Password { get; set; } = string.Empty;

	// role
	// default is member
	[Required]
	[DefaultValue("Member")]
	public string Role { get; set; } = string.Empty;
}
