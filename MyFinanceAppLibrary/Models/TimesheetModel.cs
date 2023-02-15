using System.ComponentModel.DataAnnotations;

namespace MyFinanceAppLibrary.Models;

public class TimesheetModel : BaseModel, IValidatableObject
{
#nullable disable
    public ulong Id { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Please select a company.")]
    public ulong CompanyId { get; set; }

    [Required]
    public DateTime TimeIn { get; set; }

    public int TimeBreak { get; set; }

    [Required]
    public DateTime TimeOut { get; set; }

    [Required]
    public int PayStatus { get; set; } = 0;

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

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        List<ValidationResult> results = new();

        if (TimeIn >= TimeOut)
        {
            results.Add(new ValidationResult("Punch out must be greater that Punch in", new[] { nameof(TimeOut) } ));
        }

        return results;
    }
}
