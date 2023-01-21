namespace MyFinanceAppLibrary.DataAccess.NoSql;

public interface IUserData
{
    Task CreateUser(UserModel userModel);
    Task<UserModel> GetUser(string id);
    Task<UserModel> GetUserFromAuthentication(string objectId);
    Task<List<UserModel>> GetUsersAsync();
    Task UpdateUser(UserModel userModel);
}
