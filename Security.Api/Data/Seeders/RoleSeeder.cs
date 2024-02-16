using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Security.Api.Enums;

namespace Security.Api.Data.Seeders;

public static class RoleSeeder
{
	public static void SeedRoles(this ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<IdentityRole>().HasData(
				new IdentityRole
				{
					Id = Guid.NewGuid().ToString(),
					Name = Role.Admin,
					NormalizedName = Role.Admin.ToUpperInvariant(),
				},
				new IdentityRole
				{
					Id = Guid.NewGuid().ToString(),
					Name = Role.Member,
					NormalizedName = Role.Member.ToUpperInvariant(),
				}
		);
	}
}