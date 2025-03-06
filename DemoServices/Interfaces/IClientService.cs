using DemoModels;
using Microsoft.AspNetCore.Http;

namespace DemoServices.Interfaces
{
    public interface IClientService
    {
        ClientModel? CreateClient(ClientModel model, int userId);
        ClientModel? GetClient(int clientId);
        List<ClientModel> GetClients(bool activeOnly = true, bool excludeInternal = true);
        bool UpdateClient(ClientModel model, int userId);
        bool DeleteClient(int clientId, int userId);

        bool CheckForUniqueClientName(int clientId, string clientName);
        List<KeyValuePair<int, string>> GetClientKeyValuePairs(bool activeOnly = true, bool excludeInternal = true);
        int GetCurrentClientId(HttpContext httpContext);

        bool CreateClientUser(int clientId, int userId, int userId_Source);
        bool DeleteClientUser(int clientId, int userId, int userId_Source);
    }
}
