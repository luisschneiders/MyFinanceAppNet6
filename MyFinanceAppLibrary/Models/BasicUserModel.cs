namespace MyFinanceAppLibrary.Models;

public class BasicUserModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string DisplayName { get; set; }
#nullable disable
    public BasicUserModel()
    {
    }
#nullable enable
    public BasicUserModel(UserModel userModel)
    {
        Id = userModel.Id;
        DisplayName = userModel.DisplayName;
    }
}
