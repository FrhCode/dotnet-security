using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Security.Api.Data.Seeders;
using Security.Api.Enums;

namespace Security.Api;

public class ApplicationDbContext : IdentityDbContext
{
	public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
	{
		AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
	}

	// create role admin and member
	protected override void OnModelCreating(ModelBuilder builder)
	{
		base.OnModelCreating(builder);
		builder.SeedRoles();
	}

}
