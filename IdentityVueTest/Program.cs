using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace IdentityVueTest
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var seed = args.Any(x => x == "/initializeDb");
			if (seed) args = args.Except(new[] { "/initializeDb" }).ToArray();

			var host = CreateWebHostBuilder(args).Build();

			if (seed)
			{
				var config = host.Services.GetRequiredService<IConfiguration>();
				var connectionString = config.GetConnectionString("AspIdentityConnection");
				await InitialUsers.InitializeDb(connectionString);
				return;
			}

			host.Run();
		}

		public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
			WebHost.CreateDefaultBuilder(args)
				.UseStartup<Startup>();
	}
}
