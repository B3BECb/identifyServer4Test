using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer4.Services;
using IdentityVueTest.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityVueTest.Api
{
	public class UserInfoEndPoint : Controller
	{
		// GET api/<controller>/5
		[HttpGet]
		[Route("api/v1/UserInfo")]
		public async Task<IActionResult> Get()
		{
			var userClaims = (User.Identity as ClaimsIdentity);

			var isAdmin = false;

			isAdmin |= User.IsInRole("Administrator");
			isAdmin |= User.IsInRole("Identity administrator");

			return new JsonResult(new UserInfo
			{
				UserName = userClaims.Name,
				IsAdmin = isAdmin,
			});
		}
	}
}
