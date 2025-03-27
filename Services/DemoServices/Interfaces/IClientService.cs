using DemoModels;

namespace DemoServices.Interfaces
{
    public interface IClientService
    {
        ClientModel? CreateClient(ClientModel model, int userId);
        ClientModel? GetClient(int clientId);
        List<ClientModel> GetClients(bool activeOnly = true, bool excludeInternal = true, bool resetCache = false);
        bool UpdateClient(ClientModel model, int userId);
        bool DeleteClient(int clientId, int userId);

        bool CheckForUniqueClientName(int clientId, string clientName);
        List<KeyValuePair<int, string>> GetClientKeyValuePairs(bool activeOnly = true, bool excludeInternal = true);

        bool CreateClientUser(int clientId, int userId, int userId_Source);
        List<UserModel> GetClientUsers(int clientId);
        bool DeleteClientUser(int clientId, int userId, int userId_Source);

        List<WorkItemModel> GetClientWorkItems(int clientId, bool includeActiveOnly = true);
    }
}
