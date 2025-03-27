using DemoModels;
using DemoServices.Interfaces;
using DemoUtilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DemoWebApi.Controllers
{
    // Be careful here! The decoration below means anyone can get to anything in this class.
    [AllowAnonymous]
    [Route("jwttoken")]
    public class JwtTokenController(ILogger logger, IConfiguration configuration, IServiceProvider serviceProvider) : ControllerBase
    {
        // Dependencies
        internal readonly ILogger _logger = logger;
        internal readonly IConfiguration _configuration = configuration;
        internal readonly IServiceProvider _serviceProvider = serviceProvider;
        internal readonly EmailUtility _emailUtility = new(configuration);

        [HttpGet]
        public IActionResult GetSecurityToken(string token)
        {
            var jwtToken = JwtTokenUtility.GetSecurityToken(_configuration, token);
            return Ok(jwtToken);
        }

        [HttpPost]
        public IActionResult GenerateToken(JwtTokenRequestModel request)
        {
            UserModel? user;
            using (var scope = _serviceProvider.CreateScope())
            {
                var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
                user = userService.GetUser(request.EmailAddress, request.Password);
            }

            if (user == null)
            {
                return BadRequest(new { message = "Incorrect username or password." });
            }

            var token = JwtTokenUtility.GenerateToken(_configuration, user.UserId, user.EmailAddress, user.FirstName ?? string.Empty, user.LastName ?? string.Empty);
            return Ok(token);
        }
    }
}
