using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

namespace DemoWebApi.Controllers
{
    // Be careful here! The decoration below means anyone can get to anything in this class.
    [AllowAnonymous]
    [Route("public")]
    public class PublicController : ControllerBase
    {
        [HttpGet("secretkey")]
        public string GenerateSecretKey()
        {
            var key = new byte[32];
            RandomNumberGenerator.Create().GetBytes(key);
            var base64Secret = Convert.ToBase64String(key);
            var secretKey = base64Secret.TrimEnd('=').Replace('+', '-').Replace('/', '_');
            return secretKey;
        }
    }
}
