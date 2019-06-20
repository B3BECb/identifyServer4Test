// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.IO;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using IdentityServer4;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace IdentityServer
{
	public class Startup
	{
		const string connectionString = @"Data Source=(LocalDb)\MSSQLLocalDB;database=IdentityServer4.Quickstart.EntityFramework-2.0.0;trusted_connection=yes;";

		public IHostingEnvironment Environment { get; }

		public Startup(IHostingEnvironment environment)
		{
			Environment = environment;
		}

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddMvc();

			var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

			var builder = services.AddIdentityServer()
				.AddTestUsers(Config.GetUsers())
				// this adds the config data from DB (clients, resources)
				.AddConfigurationStore(options =>
				{
					options.ConfigureDbContext = b =>
						b.UseSqlServer(connectionString,
							sql => sql.MigrationsAssembly(migrationsAssembly));
				})
				// this adds the operational data from DB (codes, tokens, consents)
				.AddOperationalStore(options =>
				{
					options.ConfigureDbContext = b =>
						b.UseSqlServer(connectionString,
							sql => sql.MigrationsAssembly(migrationsAssembly));

					// this enables automatic token cleanup. this is optional.
					options.EnableTokenCleanup = true;
				});

			if (Environment.IsDevelopment())
			{
				builder.AddDeveloperSigningCredential();
			}
			else
			{
				var dir = GetApplicationRoot();
				var path = Path.Combine(dir, "testautamserv.pfx");
				Console.WriteLine(path);
				var cert = new X509Certificate2(path, "password");
				builder.AddSigningCredential(cert);
				//throw new Exception("need to configure key material");
			}
		}

		public void Configure(IApplicationBuilder app)
		{
			if(Environment.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			// uncomment if you want to support static files
			app.UseStaticFiles();

			app.UseIdentityServer();

			// uncomment, if you wan to add an MVC-based UI
			app.UseMvcWithDefaultRoute();
		}

		public string GetApplicationRoot()
		{
			var exePath = Path.GetDirectoryName(System.Reflection
							  .Assembly.GetExecutingAssembly().CodeBase);
			Regex appPathMatcher = new Regex(@"(?<!fil)[A-Za-z]:\\+[\S\s]*?(.*)");
			var appRoot = appPathMatcher.Match(exePath).Value;
			return appRoot;
		}
	}
}