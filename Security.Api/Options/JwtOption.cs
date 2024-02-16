using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Security.Api.Options;

public class JwtOption
{
	public string Issuer { get; set; } = string.Empty;
	public string Audience { get; set; } = string.Empty;
	public string Key { get; set; } = string.Empty;
}