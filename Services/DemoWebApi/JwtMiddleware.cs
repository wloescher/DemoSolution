using DemoModels;
using DemoServices.Interfaces;
using System.Net;

namespace DemoWebApi
{
    public class JwtMiddleware(IConfiguration configuration, RequestDelegate next)
    {
        // Dependencies
        private readonly IConfiguration _configuration = configuration;
        private readonly RequestDelegate _next = next;

        public async Task Invoke(HttpContext context, IUserService userService)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (token != null)
            {
                if (token == "demo")
                {
                    var user = userService.GetUser(1);
                    if (user != null)
                    {
                        token = JwtTokenUtility.GenerateToken(_configuration, user.UserId, user.EmailAddress, user.FirstName ?? string.Empty, user.LastName ?? string.Empty);
                        context.Items["User"] = user;
                    }
                }

                // Check authetication
                var jwtToken = JwtTokenUtility.GetSecurityToken(_configuration, token);
                if (jwtToken != null)
                {
                    // Check authorization
                    var requestAudience = context.Request.Host.ToString();
                    if (!jwtToken.Audiences.Contains(requestAudience, StringComparer.InvariantCultureIgnoreCase))
                    {
                        // Unauthorized request
                        context.Response.ContentType = "application/json";
                        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        await context.Response.WriteAsJsonAsync(new { Success = false, Message = "Unauthorized access." });
                    }
                }
            }

            await _next(context);
        }
    }
}
