using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Security.Api.Dtos;

public class SignInDto
{
	[Required]
	[DefaultValue("farhanbantulm1@gmail.com")]
	public string Email { get; set; } = string.Empty;

	[Required]
	[DefaultValue("indonesia123B$")]
	public string Password { get; set; } = string.Empty;
}