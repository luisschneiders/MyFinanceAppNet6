using System.ComponentModel.DataAnnotations;

namespace MyFinanceAppLibrary.Models;

public class VehicleModel : BaseModel
{
#nullable disable
    public ulong Id { get; set; }

    [Required]
    public string Description { get; set; }

    [Required]
    public string Plate { get; set; }

    [Required]
    public int Year { get; set; }

    [Required]
    public string Carmaker { get; set; }
#nullable enable
}
