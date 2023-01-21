namespace MyFinanceAppLibrary.DataAccess.NoSql;

public interface IDbConnection
{
    string DbName { get; }
    string UserCollectionName { get; }
    MongoClient Client { get; }
    IMongoCollection<UserModel> UserCollection { get; }
}
