using System.ComponentModel.DataAnnotations;

namespace MyFinanceAppLibrary.Models;

public class BankTransactionHistoryModel : BaseModel
{
#nullable disable
    public ulong Id { get; set; }

    [Required]
    public DateTime BDate { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "The Bank field is required.")]
    public ulong BankId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "The Transaction field is required.")]
    public ulong TransactionId { get; set; }

    public string Action { get; set; }

    [Required]
    [RegularExpression(@"^\d+(.\d{1,2})?$", ErrorMessage = "Invalid amount.")]
    public decimal PreviousBalance { get; set; }

    [Required]
    [RegularExpression(@"^\d+(.\d{1,2})?$", ErrorMessage = "Invalid amount.")]
    public decimal Amount { get; set; }

    [Required]
    [RegularExpression(@"^\d+(.\d{1,2})?$", ErrorMessage = "Invalid amount.")]
    public decimal CurrentBalance { get; set; }
#nullable enable
}
