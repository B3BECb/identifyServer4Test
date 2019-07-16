using IdentityVueTest.Data;
using IdentityVueTest.Models;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VueCliMiddleware;

namespace IdentityVueTest
{
	public class Startup
	{
		public Startup(IConfiguration configuration, IHostingEnvironment environment)
		{
			Configuration = configuration;
			Environment = environment;
		}

		public IConfiguration Configuration { get; }

		public IHostingEnvironment Environment { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddDbContext<AspIdentityDbContext>(options =>
				options.UseSqlServer(Configuration.GetConnectionString("AspIdentityConnection")));

			services.AddIdentity<User, IdentityRole>()
				.AddEntityFrameworkStores<AspIdentityDbContext>()
				.AddDefaultTokenProviders();

			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

			services.Configure<IISOptions>(iis =>
			{
				iis.AuthenticationDisplayName = "Windows";
				iis.AutomaticAuthentication = false;
			});

			// In production, the React files will be served from this directory
			services.AddSpaStaticFiles(configuration =>
			{
				configuration.RootPath = "ClientApp/dist";
			});

			services.AddAntiforgery(options =>
			{
				options.FormFieldName = "XSRF-TOKEN-FIELD";
				options.HeaderName = "X-XSRF-TOKEN";
				options.SuppressXFrameOptionsHeader = false;
			});

			services.Configure<IdentityOptions>(options =>
			{
				options.Password.RequireDigit = false;
				options.Password.RequireNonAlphanumeric = false;
				options.Password.RequireUppercase = false;
				options.Password.RequireLowercase = false;
				options.Password.RequiredLength = 4;
				options.Password.RequiredUniqueChars = 1;
			});

			var builder = services.AddIdentityServer(options =>
			{
				options.Events.RaiseErrorEvents = true;
				options.Events.RaiseInformationEvents = true;
				options.Events.RaiseFailureEvents = true;
				options.Events.RaiseSuccessEvents = true;
			})
				.AddInMemoryIdentityResources(Config.GetIdentityResources())
				.AddInMemoryApiResources(Config.GetApis())
				.AddInMemoryClients(Config.GetClients())
				.AddAspNetIdentity<User>();

			if (Environment.IsDevelopment())
			{
				builder.AddDeveloperSigningCredential();
			}
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env, IAntiforgery antiforgery)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseDatabaseErrorPage();
			}
			else
			{
				app.UseExceptionHandler("/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();
			app.UseSpaStaticFiles();
			app.UseIdentityServer();

			app.Use(next => context =>
			{
				var tokens = antiforgery.GetAndStoreTokens(context);
				context.Response.Cookies.Append("XSRF-TOKEN", tokens.RequestToken,
					new CookieOptions() { HttpOnly = false });

				return next(context);
			});

			app.UseMvc(routes =>
			{
				routes.MapRoute(
					name: "default",
					template: "{controller}/{action=Index}/{id?}");
			});

			app.UseSpa(spa =>
			{
				spa.Options.SourcePath = "ClientApp/dist";

				if (env.IsDevelopment())
				{
					spa.UseVueCli(npmScript: "serve");
				}
			});
		}
	}
}
