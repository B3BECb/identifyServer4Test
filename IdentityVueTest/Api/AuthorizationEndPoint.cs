using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
		// POST api/<controller>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public void Post([FromForm]AuthorizationData data)
		{
		}
	}
}
