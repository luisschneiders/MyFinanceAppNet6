namespace MyFinanceAppLibrary.Models;

public class MultiFilterTimesheetDTO
{
    public DateTimeRange DateTimeRange { get; set; } = new();
    public List<ulong> CompanyId { get; set; } = new();
    public bool IsFilterChanged { get; set; } = false;
}
