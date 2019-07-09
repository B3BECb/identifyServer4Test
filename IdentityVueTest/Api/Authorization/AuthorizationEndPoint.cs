using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Events;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using IdentityVueTest.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityVueTest.Api
{
	public class AuthorizationData
	{
		private string _login;

		private string _password;

		private string _returnUrl;

		public string Login
		{
			get => _login;
			set
			{
				if (string.IsNullOrWhiteSpace(value))
					_login = "";
				else
					_login = value;
			}
		}

		public string Password
		{
			get => _password;
			set
			{
				if (string.IsNullOrWhiteSpace(value))
					_password = "";
				else
					_password = value;
			}
		}

		public string ReturnUrl
		{
			get => _returnUrl;
			set
			{
				if (string.IsNullOrWhiteSpace(value))
					_returnUrl = "";
				else
					_returnUrl = value;
			}
		}

		public bool RememberLogin { get; set; }
	}

	[SecurityHeaders]
	[AllowAnonymous]
	[Route("api/v1/authorization/login")]
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

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Post(AuthorizationData model)
		{
			var context = await Interaction.GetAuthorizationContextAsync(model.ReturnUrl);

			var returnUrl = !string.IsNullOrWhiteSpace(model.ReturnUrl)
				? "?ReturnUrl=" + model.ReturnUrl
				: "";

			if (ModelState.IsValid)
			{
				var result = await SignInManager.PasswordSignInAsync(model.Login, model.Password, model.RememberLogin, lockoutOnFailure: true);
				if (result.Succeeded)
				{
					var user = await UserManager.FindByNameAsync(model.Login);
					await Events.RaiseAsync(new UserLoginSuccessEvent(user.UserName, user.Id, user.UserName));

					if (context != null)
					{
						return Redirect(model.ReturnUrl);
					}

					// request for a local page
					if (Url.IsLocalUrl(model.ReturnUrl))
					{
						return Redirect(model.ReturnUrl);
					}
					else if (string.IsNullOrEmpty(model.ReturnUrl))
					{
						return Redirect("~/");
					}
					else
					{
						// user might have clicked on a malicious link - should be logged
						throw new Exception("invalid return URL");
					}
				}

				await Events.RaiseAsync(new UserLoginFailureEvent(model.Login, "invalid credentials"));
				ModelState.AddModelError(string.Empty, AccountOptions.InvalidCredentialsErrorMessage);
			}

			return Redirect("~/account/login" + returnUrl);
		}
	}
}
