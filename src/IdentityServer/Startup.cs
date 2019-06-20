// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using IdentityServer4;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace IdentityServer
{
	public class Startup
	{
		public IHostingEnvironment Environment { get; }

		public Startup(IHostingEnvironment environment)
		{
			Environment = environment;
		}

		public void ConfigureServices(IServiceCollection services)
		{
			// uncomment, if you wan to add an MVC-based UI
			//services.AddMvc().SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_2_1);
			services.AddMvc();

			var builder = services.AddIdentityServer()
				.AddInMemoryIdentityResources(Config.GetIdentityResources())
				.AddInMemoryApiResources(Config.GetApis())
				.AddInMemoryClients(Config.GetClients())
				.AddTestUsers(Config.GetUsers());

			if(Environment.IsDevelopment())
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

			//services.AddAuthentication()
			//	.AddGoogle("Google", options =>
			//	{
			//		options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;

			//		options.ClientId = "empty";
			//		options.ClientSecret = "empty";
			//	})
			//	.AddOpenIdConnect("oidc", "OpenID Connect", options =>
			//	{
			//		options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
			//		options.SignOutScheme = IdentityServerConstants.SignoutScheme;
			//		options.SaveTokens = true;

			//		options.Authority = "https://demo.identityserver.io/";
			//		options.ClientId = "implicit";

			//		options.TokenValidationParameters = new TokenValidationParameters
			//		{
			//			NameClaimType = "name",
			//			RoleClaimType = "role"
			//		};
			//	});
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