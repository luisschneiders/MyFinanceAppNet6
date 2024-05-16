namespace MyFinanceAppLibrary.Models;

public class FilterTimesheetDTO
{
#nullable disable
    public DateTimeRange DateTimeRange { get; set; }
    public ulong CompanyId { get; set; } = 0;
    public bool IsFilterChanged { get; set; } = false;
#nullable enable
}
