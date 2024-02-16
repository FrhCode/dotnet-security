using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Security.Api.Controllers;

[Route("api/homes")]
public class HomeController : ControllerBase
{
	[HttpGet]
	[Authorize]
	public IActionResult Get()
	{
		return Ok("Hello World");
	}


	[HttpPost]
	public IActionResult Post([FromBody] LocationDto locationDto)
	{
		// if (ModelState.IsValid == false)
		// 	return BadRequest(ModelState.ToDictionary());

		return Ok(locationDto);
	}

}

public class LocationDto : IValidatableObject
{
	[Required]
	public string City { get; set; } = String.Empty;

	[Required]
	public string Country { get; set; } = String.Empty;

	// list of facilities
	public List<FacilityDto> Facilities { get; set; } = new List<FacilityDto>();

	public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
	{
		var results = new List<ValidationResult>();

		if (Facilities.Count > 5)
			results.Add(new ValidationResult("Facilities cannot be more than 5", new[] { "Facilities.0" }));

		return results;
	}
}

public class FacilityDto
{
	public string Name { get; set; } = String.Empty;
	public string Description { get; set; } = String.Empty;
}