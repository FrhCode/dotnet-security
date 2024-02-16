using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Security.Api;

public static class ModelStateExtension
{
	public static Dictionary<string, string> ToDictionary(this ModelStateDictionary modelState)
	{
		return modelState.ToDictionary(
			kvp => kvp.Key.Substring(0, 1).ToLower() + kvp.Key.Substring(1),
			kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).First()
		);
	}

	public static BadRequestResponse ToBadRequestResponse(this ModelStateDictionary modelState)
	{
		return new BadRequestResponse(modelState);
	}
}
