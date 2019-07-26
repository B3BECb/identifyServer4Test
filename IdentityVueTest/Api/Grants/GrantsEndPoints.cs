using IdentityServer4.Events;
using IdentityServer4.Extensions;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityVueTest.Api
{
	[SecurityHeaders]
	[Authorize]
	[Route("api/v1/grants")]
	public class GrantsEndPoints : Controller
	{
		private IIdentityServerInteractionService Interaction { get; set; }
		private IClientStore Clients { get; set; }
		private IResourceStore Resources { get; set; }
		private IEventService Events { get; set; }

		public GrantsEndPoints(IIdentityServerInteractionService interaction,
			IClientStore clients,
			IResourceStore resources,
			IEventService events)
		{
			Interaction = interaction;
			Clients = clients;
			Resources = resources;
			Events = events;
		}

		[HttpGet]
		public async Task<GrantsViewModel> Index()
		{
			var grants = await Interaction.GetAllUserConsentsAsync();

			var list = new List<GrantViewModel>();
			foreach (var grant in grants)
			{
				var client = await Clients.FindClientByIdAsync(grant.ClientId);
				if (client != null)
				{
					var resources = await Resources.FindResourcesByScopeAsync(grant.Scopes);

					var item = new GrantViewModel()
					{
						ClientId = client.ClientId,
						ClientName = client.ClientName ?? client.ClientId,
						ClientLogoUrl = client.LogoUri,
						ClientUrl = client.ClientUri,
						Created = grant.CreationTime,
						Expires = grant.Expiration,
						IdentityGrantNames = resources.IdentityResources.Select(x => x.DisplayName ?? x.Name).ToArray(),
						ApiGrantNames = resources.ApiResources.Select(x => x.DisplayName ?? x.Name).ToArray()
					};

					list.Add(item);
				}
			}

			return new GrantsViewModel
			{
				Grants = list
			};
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Revoke(string clientId)
		{
			await Interaction.RevokeUserConsentAsync(clientId);
			await Events.RaiseAsync(new GrantsRevokedEvent(User.GetSubjectId(), clientId));

			return Redirect("/");
		}
	}
}
