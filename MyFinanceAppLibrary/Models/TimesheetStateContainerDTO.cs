namespace MyFinanceAppLibrary.Models;

public class TimesheetStateContainerDTO
{
    public List<TimesheetListDTO> Timesheets { get; set; } = new();
    public double TotalHours { get; set; }
    public decimal TotalPaid { get; set; }
    public decimal TotalAwaiting { get; set; }
}
