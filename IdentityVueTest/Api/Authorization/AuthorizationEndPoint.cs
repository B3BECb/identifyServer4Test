using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4.Events;
using IdentityServer4.Extensions;
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
		[Route("api/v1/authorization/login")]
		public async Task<IActionResult> Login(AuthorizationData model)
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

		/// <summary>
		/// Show logout page
		/// </summary>
		[HttpGet]
		public async Task<IActionResult> Logout(string logoutId)
		{
			// build a model so the logout page knows what to display
			var vm = await BuildLogoutViewModelAsync(logoutId);

			if (vm.ShowLogoutPrompt == false)
			{
				// if the request for logout was properly authenticated from IdentityServer, then
				// we don't need to show the prompt and can just log the user out directly.
				return await Logout(vm);
			}

			return View(vm);
		}

		/// <summary>
		/// Handle logout page postback
		/// </summary>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Logout(LogoutInputModel model)
		{
			// build a model so the logged out page knows what to display
			var vm = await BuildLoggedOutViewModelAsync(model.LogoutId);

			if (User?.Identity.IsAuthenticated == true)
			{
				// delete local authentication cookie
				await SignInManager.SignOutAsync();

				// raise the logout event
				await Events.RaiseAsync(new UserLogoutSuccessEvent(User.GetSubjectId(), User.GetDisplayName()));
			}

			// check if we need to trigger sign-out at an upstream identity provider
			if (vm.TriggerExternalSignout)
			{
				// build a return URL so the upstream provider will redirect back
				// to us after the user has logged out. this allows us to then
				// complete our single sign-out processing.
				string url = Url.Action("Logout", new { logoutId = vm.LogoutId });

				// this triggers a redirect to the external provider for sign-out
				return SignOut(new AuthenticationProperties { RedirectUri = url }, vm.ExternalAuthenticationScheme);
			}

			return View("LoggedOut", vm);
		}

		private async Task<LogoutViewModel> BuildLogoutViewModelAsync(string logoutId)
		{
			var vm = new LogoutViewModel { LogoutId = logoutId, ShowLogoutPrompt = AccountOptions.ShowLogoutPrompt };

			if (User?.Identity.IsAuthenticated != true)
			{
				// if the user is not authenticated, then just show logged out page
				vm.ShowLogoutPrompt = false;
				return vm;
			}

			var context = await Interaction.GetLogoutContextAsync(logoutId);
			if (context?.ShowSignoutPrompt == false)
			{
				// it's safe to automatically sign-out
				vm.ShowLogoutPrompt = false;
				return vm;
			}

			// show the logout prompt. this prevents attacks where the user
			// is automatically signed out by another malicious web page.
			return vm;
		}

		private async Task<LoggedOutViewModel> BuildLoggedOutViewModelAsync(string logoutId)
		{
			// get context information (client name, post logout redirect URI and iframe for federated signout)
			var logout = await Interaction.GetLogoutContextAsync(logoutId);

			var vm = new LoggedOutViewModel
			{
				AutomaticRedirectAfterSignOut = AccountOptions.AutomaticRedirectAfterSignOut,
				PostLogoutRedirectUri = logout?.PostLogoutRedirectUri,
				ClientName = string.IsNullOrEmpty(logout?.ClientName) ? logout?.ClientId : logout?.ClientName,
				SignOutIframeUrl = logout?.SignOutIFrameUrl,
				LogoutId = logoutId
			};

			if (User?.Identity.IsAuthenticated == true)
			{
				var idp = User.FindFirst(JwtClaimTypes.IdentityProvider)?.Value;
				if (idp != null && idp != IdentityServer4.IdentityServerConstants.LocalIdentityProvider)
				{
					var providerSupportsSignout = await HttpContext.GetSchemeSupportsSignOutAsync(idp);
					if (providerSupportsSignout)
					{
						if (vm.LogoutId == null)
						{
							// if there's no current logout context, we need to create one
							// this captures necessary info from the current logged in user
							// before we signout and redirect away to the external IdP for signout
							vm.LogoutId = await Interaction.CreateLogoutContextAsync();
						}

						vm.ExternalAuthenticationScheme = idp;
					}
				}
			}

			return vm;
		}
	}
}
