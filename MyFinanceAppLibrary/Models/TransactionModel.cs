using System.ComponentModel.DataAnnotations;

namespace MyFinanceAppLibrary.Models;

public class TransactionModel : BaseModel
{
#nullable disable
    public ulong Id { get; set; }

    public ulong TLink { get; set; }

    [Required]
    public DateTime TDate { get; set; }

    [Required]
    public ulong TFromBank { get; set; }

    [Required]
    public ulong TCategoryType { get; set; }

    [Required]
    public string TAction { get; set; }

    [Required]
    public string TLabel { get; set; }

    [Required]
    [RegularExpression(@"^\d+(.\d{1,2})?$", ErrorMessage = "Invalid value")]
    public decimal Amount { get; set; }

    [Required]
    public string Comments { get; set; }
#nullable enable
}
