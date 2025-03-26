﻿using DemoModels;
using Microsoft.AspNetCore.Http;

namespace DemoServices.Interfaces
{
    public interface IUserService
    {
        int GetCurrentUserId(HttpContext httpContext);

        UserModel? CreateUser(UserModel model, int userId, out string errorMessage);
        UserModel? GetUser(int userId);
        List<UserModel> GetUsers(bool activeOnly = true, bool excludeInternal = true, bool resetCache = false);
        bool UpdateUser(UserModel model, int userId, out string errorMessage);
        bool DeleteUser(int userId, int userId_Source);

        bool CheckForUniqueUserEmailAddress(int userId, string emailAddress);
        List<KeyValuePair<int, string>> GetUserKeyValuePairs(bool activeOnly = true, bool excludeInternal = true);

        UserModel? GetUser(string emailAddress, string password);
    }
}
