using EventEdu.Domain.Entities.Identity;
using EventEdu.Domain.Entities.Role;
using Microsoft.AspNetCore.Identity;

namespace EventEdu.Persistence;

public static class DbInitializer
{
	public static async Task SeedAsync(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
	{
		await SeedRolesAsync(roleManager);
		await SeedUsersAsync(userManager);
	}

	private static async Task SeedRolesAsync(RoleManager<AppRole> roleManager)
	{
		foreach (RoleModel role in Enum.GetValues(typeof(RoleModel)))
			if (!await roleManager.RoleExistsAsync(role.ToString()))
			{
				var appRole = new AppRole { Name = role.ToString() };
				await roleManager.CreateAsync(appRole);
			}
	}

	private static async Task SeedUsersAsync(UserManager<AppUser> userManager)
	{
		await CreateUserAsync(userManager, "admin", "admin@email.com", "Admin123@", RoleModel.Admin);
	}

	private static async Task CreateUserAsync(UserManager<AppUser> userManager,
		string userName, string email, string password, RoleModel role)
	{
		if (await userManager.FindByNameAsync(userName) == null)
		{
			var user = new AppUser
			{
				Id = Guid.NewGuid().ToString(),

				UserName = userName,
				EmailConfirmed = true,
				Email = email
			};

			var result = await userManager.CreateAsync(user, password);
			if (!result.Succeeded)
			{
				var errors = string.Join("; ", result.Errors);
				throw new Exception($"User creation failed: {errors}");
			}

			await userManager.AddToRoleAsync(user, role.ToString());
		}
	}
}



