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

        [HttpGet("{userId}/clients")]
        public IActionResult GetClients(int userId)
        {
            List<ClientModel> models;
            using (var scope = _serviceProvider.CreateScope())
            {
                var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
                models = userService.GetUserClients(userId);
            }
            return Ok(models);
        }

        [HttpGet("{userId}/checkemailaddress")]
        public IActionResult CheckForUniqueEmailAddress(int userId, string emailAddress)
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
        public IActionResult CreateUser([FromBody] UserModel model)
        {
            UserModel? user;
            using (var scope = _serviceProvider.CreateScope())
            {
                var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
                var currentUserId = userService.GetCurrentUserId(HttpContext);
                user = userService.CreateUser(model, currentUserId, out string errorMessage);
            }
            return Ok(user);
        }

        [HttpPut("{userId}")]
        public IActionResult UpdateUser(int userId, [FromBody] UserModel model)
        {
            bool result;
            using (var scope = _serviceProvider.CreateScope())
            {
                var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
                var currentUserId = userService.GetCurrentUserId(HttpContext);
                result = userService.UpdateUser(model, currentUserId, out string errorMessage);
            }
            return Ok(result);
        }

        [HttpDelete("{userId}")]
        public IActionResult DeleteUser(int userId)
        {
            bool result;
            using (var scope = _serviceProvider.CreateScope())
            {
                var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
                var currentUserId = userService.GetCurrentUserId(HttpContext);
                result = userService.DeleteUser(userId, currentUserId);
            }
            return Ok(result);
        }

        [HttpPut("{userId}/client/{clientId}")]
        public IActionResult AddClient(int userId, int clientId)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
                var currentUserId = userService.GetCurrentUserId(HttpContext);

                var clientService = scope.ServiceProvider.GetRequiredService<IClientService>();
                clientService.CreateClientUser(clientId, userId, currentUserId);
            }

            return Ok(true);
        }

        [HttpDelete("{userId}/client/{clientId}")]
        public IActionResult DeleteClient(int userId, int clientId)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
                var currentUserId = userService.GetCurrentUserId(HttpContext);

                var clientService = scope.ServiceProvider.GetRequiredService<IClientService>();
                clientService.DeleteClientUser(clientId, userId, currentUserId);
            }

            return Ok(true);
        }
    }
}
