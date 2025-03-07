using DemoModels;

namespace DemoServices.Interfaces
{
    public interface IUserService
    {
        UserModel? CreateUser(UserModel model, int userId);
        UserModel? GetUser(int userId);
        List<UserModel> GetUsers(bool activeOnly = true, bool excludeInternal = true);
        bool UpdateUser(UserModel model, int userId);
        bool DeleteUser(int userId, int userId_Source);

        bool CheckForUniqueUserEmailAddress(int userId, string emailAddress);
        List<KeyValuePair<int, string>> GetUserKeyValuePairs(bool activeOnly = true, bool excludeInternal = true);
    }
}
