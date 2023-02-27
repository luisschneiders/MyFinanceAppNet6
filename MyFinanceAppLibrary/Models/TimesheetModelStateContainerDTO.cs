namespace MyFinanceAppLibrary.Models;

public class TimesheetModelStateContainerDTO
{
    public List<TimesheetModelListDTO> Timesheets { get; set; } = new();
    public double TotalHours { get; set; }
    public decimal TotalPaid { get; set; }
    public decimal TotalAwaiting { get; set; }
}
