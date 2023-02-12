using System.ComponentModel.DataAnnotations;

namespace MyFinanceAppLibrary.Models;

public class TimesheetModelListDTO : BaseModel
{
#nullable disable
    public ulong Id { get; }

    public string Description { get; }

    public DateTime TimeIn { get; }

    public int TimeBreak { get; }

    public DateTime TimeOut { get; }

    public int PayStatus { get; set; }

    public string Comments { get; }
#nullable enable

    public TimeSpan HoursWorked { get; }

    public decimal TotalAmount { get; }
}
