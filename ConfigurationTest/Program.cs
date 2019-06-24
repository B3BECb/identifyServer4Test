using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;

namespace ConfigurationTest
{
	public class Program
	{
		public static void Main(string[] args)
		{
			CreateHostBuilder(args).Build().Run();
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder.UseStartup<Startup>();
				})
			.ConfigureAppConfiguration((hostContext, configBuilder) =>
			{
				var commonAppData = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);

				var appFolder = Path.Combine(commonAppData, "Mallenom", "TestApp");

				var configFolder = Path.Combine(appFolder, "Configuration");

				if (!Directory.Exists(configFolder))
				{
					Directory.CreateDirectory(configFolder);
				}

				configBuilder.AddJsonFile(Path.Combine(configFolder, "config.json"));
			});
	}
}