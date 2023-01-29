using System.ComponentModel.DataAnnotations;

namespace MyFinanceAppLibrary.Models;

public class BankModel : BaseModel
{
#nullable disable
    public ulong Id { get; set; }

    [Required]
    public string Description { get; set; }

    [Required]
    public string Account { get; set; }

    [Required]
    [RegularExpression(@"^\d+(.\d{1,2})?$", ErrorMessage = "Invalid value")]
    public decimal InitialBalance { get; set; }

    [Required]
    [RegularExpression(@"^\d+(.\d{1,2})?$", ErrorMessage = "Invalid value")]
    public decimal CurrentBalance { get; set; }
#nullable enable
}
