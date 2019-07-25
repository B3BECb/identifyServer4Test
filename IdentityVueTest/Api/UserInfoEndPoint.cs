using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityVueTest.Api
{
	[Authorize]
	public class UserInfoEndPoint : Controller
	{
		class UserInfo
		{
			public string UserName { get; set; }
		}

		private IProfileService ProfileService { get; }

		public UserInfoEndPoint(IProfileService profileService)
		{
			ProfileService = profileService;
		}

		// GET api/<controller>/5
		[HttpGet]
		[Route("api/v1/UserInfo")]
		public IActionResult Get()
		{
			var user = (HttpContext.User.Identity as ClaimsIdentity);

			return new JsonResult(new UserInfo
			{
				UserName = user.Name,
			});
		}
	}
}
