using System.ComponentModel.DataAnnotations;

namespace MyFinanceAppLibrary.Models;

public class TripModel : BaseModel
{
#nullable disable
    public ulong Id { get; set; }

    [Required]
    public DateTime TDate { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "The Vehicle field is required.")]
    public ulong VehicleId { get; set; }

    [Required]
    [RegularExpression(@"^\d+(.\d{1,2})?$", ErrorMessage = "Invalid value")]
    [Range(0.01, int.MaxValue, ErrorMessage = "The Distance must be greater than 0.00.")]
    public decimal Distance { get; set; }
#nullable enable
}
