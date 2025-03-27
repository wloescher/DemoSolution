using DemoModels;

namespace DemoServices.Interfaces
{
    public interface IWorkItemService
    {
        WorkItemModel? CreateWorkItem(WorkItemModel model, int userId);
        WorkItemModel? GetWorkItem(int workItemId);
        List<WorkItemModel> GetWorkItems(bool activeOnly = true, bool excludeInternal = true);
        bool UpdateWorkItem(WorkItemModel model, int userId);
        bool DeleteWorkItem(int workItemId, int userId);

        bool CheckForUniqueTitle(int workItemId, string clientName);
        List<KeyValuePair<int, string>> GetWorkItemKeyValuePairs(bool activeOnly = true, bool excludeInternal = true);
 
         bool CreateWorkItemUser(int workItemId, int userId, int userId_Source);
        List<UserModel> GetWorkItemUsers(int workItemId);
        bool DeleteWorkItemUser(int workItemId, int userId, int userId_Source);
    }
}
