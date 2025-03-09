using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

namespace DemoWebApi.Controllers
{
    // Be careful here! The decoration below means anyone can get to anything in this class.
    [AllowAnonymous]
    [Route("public")]
    public class PublicController(ILogger<PublicController> logger, IConfiguration configuration, IServiceProvider serviceProvider)
        : BaseController(logger, configuration, serviceProvider)
    {
        [HttpGet("secretkey")]
        public IActionResult GenerateSecretKey()
        {
            var key = new byte[32];
            RandomNumberGenerator.Create().GetBytes(key);
            var base64Secret = Convert.ToBase64String(key);
            var urlEncoded = base64Secret.TrimEnd('=').Replace('+', '-').Replace('/', '_');
            return Ok(urlEncoded);
        }
    }
}
