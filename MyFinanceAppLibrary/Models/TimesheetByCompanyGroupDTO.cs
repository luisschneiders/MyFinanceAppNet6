namespace MyFinanceAppLibrary.Models;

public class TimesheetByCompanyGroupDTO
{
#nullable disable
    public string Description { get; set; }
    public double TotalWorkHours { get; set; }
    public TimeSpan TotalOvertime { get; set; }
    public decimal TotalAmount { get; set; }
    public List<TimesheetListDTO> Timesheets { get; set; } = new();
#nullable enable
}
