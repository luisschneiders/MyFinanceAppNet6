using System;
namespace MyFinanceAppLibrary.Models;

public class UserModel
{
#nullable disable
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string ObjectIdentifier { get; set; } // For Azure AD B2C
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string DisplayName { get; set; }
    public string EmailAddress { get; set; }
#nullable enable
}
