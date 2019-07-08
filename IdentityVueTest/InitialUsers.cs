using IdentityModel;
using IdentityVueTest.Data;
using IdentityVueTest.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityVueTest
{
	public class InitialUsers
	{
		public static async Task InitializeDb(string connectionString)
		{
			var services = new ServiceCollection();
			services.AddDbContext<AspIdentityDbContext>(options =>
			   options.UseSqlServer(connectionString));

			services.AddIdentity<User, IdentityRole>()
				.AddEntityFrameworkStores<AspIdentityDbContext>()
				.AddDefaultTokenProviders();

			services.Configure<IdentityOptions>(options =>
			{
				options.Password.RequireDigit = false;
				options.Password.RequireNonAlphanumeric = false;
				options.Password.RequireUppercase = false;
				options.Password.RequireLowercase = false;
				options.Password.RequiredLength = 4;
				options.Password.RequiredUniqueChars = 1;
			});

			using (var serviceProvider = services.BuildServiceProvider())
			{
				using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
				{
					var context = scope.ServiceProvider.GetService<AspIdentityDbContext>();
					context.Database.Migrate();

					var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
					var admin = await userMgr.FindByNameAsync("admin");
					if(admin == null)
					{
						admin = new User
						{
							UserName = "admin",
						};
						var result = await userMgr.CreateAsync(admin, "admin");
						if(!result.Succeeded)
						{
							throw new Exception(result.Errors.First().Description);
						}

						result = await userMgr.AddClaimsAsync(admin, new Claim[]
						{
							new Claim(JwtClaimTypes.Name, "Identity Admin"),
						});

						if(!result.Succeeded)
						{
							throw new Exception(result.Errors.First().Description);
						}
						Console.WriteLine("Admin created");
					}
					else
					{
						Console.WriteLine("Admin already exist");
					}
				}
			}
		}
	}
}
