using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using IdentityVueTest.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IdentityVueTest.Api
{
	public class AuthorizationData
	{
		public string Login { get; set; }

		public string Password { get; set; }
	}

	[SecurityHeaders]
	[AllowAnonymous]
	[Route("api/v1/login")]
	public class AuthorizationEndPoint : Controller
	{
		private UserManager<User> UserManager { get; set; }

		private SignInManager<User> SignInManager { get; set; }

		private IIdentityServerInteractionService Interaction { get; set; }

		private IClientStore ClientStore { get; set; }

		private IAuthenticationSchemeProvider SchemeProvider { get; set; }

		private IEventService Events { get; set; }

		public AuthorizationEndPoint(UserManager<User> userManager,
			SignInManager<User> signInManager,
			IIdentityServerInteractionService interaction,
			IClientStore clientStore,
			IAuthenticationSchemeProvider schemeProvider,
			IEventService events)
		{
			UserManager = userManager;
			SignInManager = signInManager;
			Interaction = interaction;
			ClientStore = clientStore;
			SchemeProvider = schemeProvider;
			Events = events;
		}

		// POST api/<controller>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Post([FromForm]AuthorizationData data)
		{
			var context = await Interaction.GetAuthorizationContextAsync(model.ReturnUrl);
		}
	}
}
