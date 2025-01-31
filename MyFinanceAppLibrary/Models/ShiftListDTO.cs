namespace MyFinanceAppLibrary.Models;

public class ShiftListDTO : BaseModel
{
#nullable disable
    public ulong Id { get; }

    public ulong CompanyId { get; }

    public string Description { get; }

    public DateTime SDate { get; }
#nullable enable
}
