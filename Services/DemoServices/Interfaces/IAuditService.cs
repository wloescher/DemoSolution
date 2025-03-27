using DemoModels;
using DemoRepository.Entities;

namespace DemoServices.Interfaces
{
    public interface IAuditService
    {
        List<AuditModel> GetClientAudits(int clientId);
        bool CreateClient(Client entity, int userId);
        bool UpdateClient(Client entityBefore, Client entityAfter, int userId);
        bool DeleteClient(Client entityBefore, Client entityAfter, int userId);

        List<AuditModel> GetClientUserAudits(int clientUserId);
        bool CreateClientUser(ClientUser entity, int userId);
        bool DeleteClientUser(ClientUser entity, int userId);

        List<AuditModel> GetUserAudits(int userId);
        bool CreateUser(User entity, int userId);
        bool UpdateUser(User entityBefore, User entityAfter, int userId);
        bool DeleteUser(User entityBefore, User entityAfter, int userId);

        List<AuditModel> GetWorkItemAudits(int workItemId);
        bool CreateWorkItem(WorkItem entity, int userId);
        bool UpdateWorkItem(WorkItem entityBefore, WorkItem entityAfter, int userId);
        bool DeleteWorkItem(WorkItem entityBefore, WorkItem entityAfter, int userId);

        List<AuditModel> GetWorkItemUserAudits(int workItemUserId);
        bool CreateWorkItemUser(WorkItemUser entity, int userId);
        bool DeleteWorkItemUser(WorkItemUser entity, int userId);

    }
}
