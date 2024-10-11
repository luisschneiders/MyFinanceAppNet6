using System.ComponentModel.DataAnnotations;

namespace MyFinanceAppLibrary.Models;

public class TaxCategoryModel : BaseModel
{
    public ulong Id { get; set; }

    [Required]
    public string Description { get; set; } = string.Empty;

    [Required]
    public decimal Rate { get; set; } = decimal.Zero;
}
