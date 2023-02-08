using System.ComponentModel.DataAnnotations;

namespace MyFinanceAppLibrary.Models;

public class TimesheetModel : BaseModel
{
#nullable disable
    public ulong Id { get; set; }

    [Required]
    public ulong CompanyId { get; set; }

    [Required]
    public DateTime TimeIn { get; set; }

    public int TimeBreak { get; set; }

    [Required]
    public DateTime TimeOut { get; set; }

    [Required]
    public string PayStatus { get; set; }

    [Required]
    [RegularExpression(@"^\d+(.\d{1,2})?$", ErrorMessage = "Invalid value")]
    public decimal HourRate { get; set; }

    public string Comments { get; set; }
#nullable enable

    public TimeSpan HoursWorked
    {
        get
        {
            var hoursWorked = TimeOut - TimeIn;
            var timeBreak = TimeSpan.FromMinutes(TimeBreak);
            var result = hoursWorked.Subtract(timeBreak);

            return result;
        }
    }

    public decimal TotalAmount
    {
        get
        {
            var seconds = HoursWorked.TotalHours;
            var total = (((decimal)seconds) * HourRate);
            return total;
        }
    }
}
