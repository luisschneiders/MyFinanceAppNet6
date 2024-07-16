namespace MyFinanceAppLibrary.Models;

public class TimesheetCalendarDTO
{
#nullable disable
    public DateTime TDate { get; set; }
    public string CompanyDescription { get; set; }
    public int PayStatus { get; set; }
    public decimal TotalAmount { get; set; }
#nullable enable
}
