using DemoModels;
using DemoServices.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DemoWebApi.Controllers
{
    [Route("client")]
    public class ClientController(ILogger<ClientController> logger, IConfiguration configuration, IServiceProvider serviceProvider)
        : BaseController(logger, configuration, serviceProvider)
    {
        [HttpGet]
        public IActionResult GetClientList()
        {
            List<GenericListItemModel> clientList = new();
            using (var scope = _serviceProvider.CreateScope())
            {
                var clientService = scope.ServiceProvider.GetRequiredService<IClientService>();
                foreach (var client in clientService.GetClients())
                {
                    clientList.Add(new GenericListItemModel
                    {
                        Id = client.ClientId,
                        Name = client.Name
                    });
                }
            }

            // Add empty item
            clientList.Insert(0, new GenericListItemModel { Id = 0, Name = "Select..." });
            return Ok(clientList);
        }

        [HttpGet("{clientId}")]
        public IActionResult GetClient(int clientId)
        {
            ClientModel? client;
            using (var scope = _serviceProvider.CreateScope())
            {
                var clientService = scope.ServiceProvider.GetRequiredService<IClientService>();
                client = clientService.GetClient(clientId);
            }
            return Ok(client);
        }

        [HttpGet("{clientId}/workitems")]
        public IActionResult GetWorkItems(int clientId)
        {
            List<WorkItemModel> models;
            using (var scope = _serviceProvider.CreateScope())
            {
                var clientService = scope.ServiceProvider.GetRequiredService<IClientService>();
                models = clientService.GetClientWorkItems(clientId);
            }
            return Ok(models);
        }

        [HttpGet("{clientId}/users")]
        public IActionResult GetUsers(int clientId)
        {
            List<UserModel> models;
            using (var scope = _serviceProvider.CreateScope())
            {
                var clientService = scope.ServiceProvider.GetRequiredService<IClientService>();
                models = clientService.GetClientUsers(clientId);
            }
            return Ok(models);
        }

        [HttpPost]
        public IActionResult CreateClient([FromBody] ClientModel model)
        {
            ClientModel? client;
            using (var scope = _serviceProvider.CreateScope())
            {
                var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
                var currentUserId = userService.GetCurrentUserId(HttpContext);

                var clientService = scope.ServiceProvider.GetRequiredService<IClientService>();
                client = clientService.CreateClient(model, currentUserId);
            }
            return Ok(client);
        }

        [HttpPut("{clientId}")]
        public IActionResult UpdateClient(int clientId, [FromBody] ClientModel model)
        {
            bool result;
            using (var scope = _serviceProvider.CreateScope())
            {
                var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
                var currentUserId = userService.GetCurrentUserId(HttpContext);

                var clientService = scope.ServiceProvider.GetRequiredService<IClientService>();
                result = clientService.UpdateClient(model, currentUserId);
            }
            return Ok(result);
        }

        [HttpDelete("{clientId}")]
        public IActionResult DeleteClient(int clientId)
        {
            bool result;
            using (var scope = _serviceProvider.CreateScope())
            {
                var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
                var currentUserId = userService.GetCurrentUserId(HttpContext);

                var clientService = scope.ServiceProvider.GetRequiredService<IClientService>();
                result = clientService.DeleteClient(clientId, currentUserId);
            }
            return Ok(result);
        }

        [HttpGet("{clientId}/checkname")]
        public IActionResult CheckForUniqueName(int clientId, string name)
        {
            bool result;
            using (var scope = _serviceProvider.CreateScope())
            {
                var clientService = scope.ServiceProvider.GetRequiredService<IClientService>();
                result = clientService.CheckForUniqueClientName(clientId, name);
            }
            return Ok(result);
        }

        [HttpPut("{clientId}/user/{userId}")]
        public IActionResult AddUser(int clientId, int userId)
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

        [HttpDelete("{clientId}/user/{userId}")]
        public IActionResult DeleteUser(int clientId, int userId)
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
