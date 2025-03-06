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
    }
}
