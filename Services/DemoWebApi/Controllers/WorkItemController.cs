using DemoModels;
using DemoServices.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DemoWebApi.Controllers
{
    [Route("workitem")]
    public class WorkItemController(ILogger<WorkItemController> logger, IConfiguration configuration, IServiceProvider serviceProvider)
        : BaseController(logger, configuration, serviceProvider)
    {
        [HttpGet]
        public IActionResult GetWorkItemList()
        {
            List<GenericListItemModel> workItemList = new();
            using (var scope = _serviceProvider.CreateScope())
            {
                var workItemService = scope.ServiceProvider.GetRequiredService<IWorkItemService>();
                foreach (var workItem in workItemService.GetWorkItems())
                {
                    workItemList.Add(new GenericListItemModel
                    {
                        Id = workItem.WorkItemId,
                        Name = workItem.Title
                    });
                }
            }

            // Add empty item
            workItemList.Insert(0, new GenericListItemModel { Id = 0, Name = "Select..." });
            return Ok(workItemList);
        }

        [HttpGet("{workItemId}")]
        public IActionResult GetWorkItem(int workItemId)
        {
            WorkItemModel? workItem;
            using (var scope = _serviceProvider.CreateScope())
            {
                var workItemService = scope.ServiceProvider.GetRequiredService<IWorkItemService>();
                workItem = workItemService.GetWorkItem(workItemId);
            }
            return Ok(workItem);
        }

        [HttpPost]
        public IActionResult CreateWorkItem([FromBody] WorkItemModel model)
        {
            WorkItemModel? workItem;
            using (var scope = _serviceProvider.CreateScope())
            {
                var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
                var currentUserId = userService.GetCurrentUserId(HttpContext);

                var workItemService = scope.ServiceProvider.GetRequiredService<IWorkItemService>();
                workItem = workItemService.CreateWorkItem(model, currentUserId);
            }
            return Ok(workItem);
        }

        [HttpPut("{workItemId}")]
        public IActionResult UpdateWorkItem(int workItemId, [FromBody] WorkItemModel model)
        {
            bool result;
            using (var scope = _serviceProvider.CreateScope())
            {
                var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
                var currentUserId = userService.GetCurrentUserId(HttpContext);

                var workItemService = scope.ServiceProvider.GetRequiredService<IWorkItemService>();
                result = workItemService.UpdateWorkItem(model, currentUserId);
            }
            return Ok(result);
        }

        [HttpDelete("{workItemId}")]
        public IActionResult DeleteWorkItem(int workItemId)
        {
            bool result;
            using (var scope = _serviceProvider.CreateScope())
            {
                var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
                var currentUserId = userService.GetCurrentUserId(HttpContext);

                var workItemService = scope.ServiceProvider.GetRequiredService<IWorkItemService>();
                result = workItemService.DeleteWorkItem(workItemId, currentUserId);
            }
            return Ok(result);
        }

        [HttpGet("{workItemId}/checktitle")]
        public IActionResult CheckForUniqueTitle(int workItemId, string title)
        {
            bool result;
            using (var scope = _serviceProvider.CreateScope())
            {
                var workItemService = scope.ServiceProvider.GetRequiredService<IWorkItemService>();
                result = workItemService.CheckForUniqueWorkItemTitle(workItemId, title);
            }
            return Ok(result);
        }
    }
}
