using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Security.Api.Options;

public class FacebookOauthOption
{
	public string AppId { get; set; } = String.Empty;
	public string AppSecret { get; set; } = String.Empty;
}