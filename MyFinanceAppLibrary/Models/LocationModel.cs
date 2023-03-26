using System.ComponentModel.DataAnnotations;

namespace MyFinanceAppLibrary.Models;

public class LocationModel : BaseModel
{
#nullable disable
    public ulong Id { get; set; }

    [Required]
    public string Address { get; set; }

    public decimal Latitude { get; set; }

    public decimal Longitude { get; set; }
#nullable enable
}
