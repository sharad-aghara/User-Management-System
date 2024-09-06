using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using NuGet.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Linq;
using System.Security.Claims;
using UMS.Core;

namespace DK.Web.Middlewares
{
	public class AuthMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly IConfiguration _configuration;
		
		public AuthMiddleware(RequestDelegate next, IConfiguration configuration)
		{
			_next = next;
			_configuration = configuration;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			var authToken = context.Request.Cookies[Constant.JWT_COOKIE_NAME];

			
			if (authToken != null)
			{
				VerifyAuthToken(context, authToken);
			}

			await _next(context);
		}

		private void VerifyAuthToken(HttpContext context, string token)
		{
			try
			{
				var tokenHandler = new JwtSecurityTokenHandler();
				var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);

				tokenHandler.ValidateToken(token,
					new TokenValidationParameters
					{
						ValidateIssuer = false,
						ValidateAudience = false,
						ValidateIssuerSigningKey = true,
						IssuerSigningKey = new SymmetricSecurityKey(key)
					}, 
					out SecurityToken validatedToken
				);

				var jwtToken = (JwtSecurityToken)validatedToken;
				var claims = jwtToken.Claims.ToList();

				var identity = new ClaimsIdentity(claims, "jwt");
				context.User = new ClaimsPrincipal(identity);
			}
			catch
			{

			}
		}
	}
}
