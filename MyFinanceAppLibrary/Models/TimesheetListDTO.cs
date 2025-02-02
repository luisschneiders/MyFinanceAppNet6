namespace MyFinanceAppLibrary.Models;

public class TimesheetListDTO : BaseModel
{
#nullable disable
    public ulong Id { get; }

    public ulong CompanyId { get; }

    public string Description { get; }

    public DateTime TimeIn { get; }

    public int TimeBreak { get; }

    public DateTime TimeOut { get; }

    public int PayStatus { get; set; }

    public string Comments { get; }

    public TimeSpan Overtime { get; }
#nullable enable

    public TimeSpan HoursWorked { get; }

    public decimal TotalAmount { get; }
}
