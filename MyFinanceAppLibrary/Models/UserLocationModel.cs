namespace MyFinanceAppLibrary.Models;

public class UserLocationModel : BaseModel
{
#nullable disable
    public ulong Id { get; set; }

    public LocationModel Location { get; set; } = new();
#nullable enable
}
