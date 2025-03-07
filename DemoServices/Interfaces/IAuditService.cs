using DemoRepository.Entities;

namespace DemoServices.Interfaces
{
    public interface IAuditService
    {
        bool CreateClient(Client entity, int userId);
        bool UpdateClient(Client entityBefore, Client entityAfter, int userId);
        bool DeleteClient(Client entityBefore, Client entityAfter, int userId);

        bool CreateClientUser(ClientUser entity, int userId);
        bool DeleteClientUser(ClientUser entity, int userId);

        bool CreateUser(User entity, int userId);
        bool UpdateUser(User entityBefore, User entityAfter, int userId);
        bool DeleteUser(User entityBefore, User entityAfter, int userId);

        bool CreateWorkItem(WorkItem entity, int userId);
        bool UpdateWorkItem(WorkItem entityBefore, WorkItem entityAfter, int userId);
        bool DeleteWorkItem(WorkItem entityBefore, WorkItem entityAfter, int userId);
    }
}
