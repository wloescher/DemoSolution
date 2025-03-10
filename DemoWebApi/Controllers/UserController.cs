using DemoModels;
using DemoServices.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Web;

namespace DemoWebApi.Controllers
{
    [Route("user")]
    public class UserController : BaseController
    {
        // Dependencies
        private readonly IWebHostEnvironment _webHostEnvironment;

        public UserController(ILogger<UserController> logger, IConfiguration configuration, IServiceProvider serviceProvider, IWebHostEnvironment webHostEnvironment)
            : base(logger, configuration, serviceProvider)
        {
            // Dependencies
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public IActionResult GetUsers()
        {
            List<UserModel> users;
            using (var scope = _serviceProvider.CreateScope())
            {
                var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
                users = userService.GetUsers();
            }
            return Ok(users);
        }

        [HttpGet("{userId}")]
        public IActionResult GetUser(int userId)
        {
            UserModel? user;
            using (var scope = _serviceProvider.CreateScope())
            {
                var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
                user = userService.GetUser(userId);
            }
            return Ok(user);
        }

        [HttpGet("list")]
        public IActionResult GetUserList()
        {
            List<UserModel> users;
            using (var scope = _serviceProvider.CreateScope())
            {
                var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
                users = userService.GetUsers();
            }

            // Create collection of users
            var userList = new List<GenericListItemModel>();
            foreach (var user in users)
            {
                userList.Add(new GenericListItemModel { Id = user.UserId, Name = user.FullName });
            }

            // Add empty item
            userList.Insert(0, new GenericListItemModel { Id = 0, Name = "Select..." });
            return Ok(userList);
        }

        [HttpGet("{userId}/checkemailaddressduplicate")]
        public IActionResult CheckEmailAddressDuplicate(int userId, string emailAddress)
        {
            List<UserModel> users;
            using (var scope = _serviceProvider.CreateScope())
            {
                var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
                users = userService.GetUsers();
            }

            var result = !users.Any(x => x.UserId != userId && x.EmailAddress == HttpUtility.UrlDecode(emailAddress).Trim());
            return Ok(result);
        }

        [HttpPost()]
        public IActionResult CreateUser([FromBody] UserModel user)
        {
            string errorMessage;
            using (var scope = _serviceProvider.CreateScope())
            {
                var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
                var currentUserId = userService.GetCurrentUserId(HttpContext);
                userService.CreateUser(user, currentUserId, out errorMessage);
            }
            return Ok(errorMessage);
        }

        [HttpPut("{userId}")]
        public IActionResult UpdateUser(int userId, [FromBody] UserModel user)
        {
            string errorMessage;
            using (var scope = _serviceProvider.CreateScope())
            {
                var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
                var currentUserId = userService.GetCurrentUserId(HttpContext);
                userService.UpdateUser(user, currentUserId, out errorMessage);
            }
            return Ok(errorMessage);
        }

        [HttpDelete("{userId}")]
        public IActionResult DeleteUser(int userId)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
                var currentUserId = userService.GetCurrentUserId(HttpContext);
                userService.DeleteUser(userId, currentUserId);
            }
            return Ok(true);
        }
    }
}
