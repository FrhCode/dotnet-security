using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;

namespace Security.Api;

public class BadRequestResponse
{
	public int StatusCode { get; } = 400;
	public string Message { get; } = "Bad Request";
	public IDictionary<string, string> Errors { get; }

	public BadRequestResponse(ModelStateDictionary modelState)
	{
		Errors = modelState.ToDictionary();
	}
}
