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
                // Check authetication
                var jwtToken = JwtTokenUtility.GetSecurityToken(_configuration, token);
                if (jwtToken != null)
                {
                    // Check authorization
                    var requestAudience = context.Request.Path.ToString().Split('/')[1];
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
