using System.ComponentModel.DataAnnotations;

namespace MyFinanceAppLibrary.Models;

public class TripModel : BaseModel
{
#nullable disable
    public ulong Id { get; set; }

    [Required]
    public DateTime TDate { get; set; }

    [Required]
    public ulong VehicleId { get; set; }

    [Required]
    [RegularExpression(@"^\d+(.\d{1,2})?$", ErrorMessage = "Invalid value")]
    public decimal Distance { get; set; }
#nullable enable
}
