namespace MyFinanceAppLibrary.Models;

public class ShiftListDTO
{
#nullable disable
    public ulong Id { get; }

    public DateTime SDate { get; }

    public ulong CompanyId { get; }

    public bool IsAvailable { get; }

    public string Description { get; }

#nullable enable
}
