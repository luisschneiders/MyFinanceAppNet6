using Microsoft.Extensions.Configuration;

namespace MyFinanceAppLibrary.DataAccess.NoSql;

public class MongoDbConnection : IDbConnection
{
    private readonly IConfiguration _config;
    private readonly IMongoDatabase _db;
    private readonly string _connectionId = "MongoDBDev";

    public string DbName { get; private set; }
    public string UserCollectionName { get; private set; } = "users";

    public MongoClient Client { get; private set; }
    public IMongoCollection<UserModel> UserCollection { get; private set; }

    public MongoDbConnection(IConfiguration config)
    {
        _config = config;
        Client = new MongoClient(_config.GetConnectionString(_connectionId));

        // Connection to DB
        DbName = _config["DatabaseName"];
        _db = Client.GetDatabase(DbName);

        // Connection to collections
        UserCollection = _db.GetCollection<UserModel>(UserCollectionName);
    }
}
