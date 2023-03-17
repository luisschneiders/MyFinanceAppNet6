using System.ComponentModel.DataAnnotations;

namespace MyFinanceAppLibrary.Models;

public class ExpenseModel : BaseModel, IValidatableObject
{
#nullable disable
    public ulong Id { get; set; }

    [Required]
    public DateTime EDate { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "The Bank field is required.")]
    public ulong BankId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "The Expense field is required.")]
    public ulong ECategoryId { get; set; }

    [Required]
    public string Comments { get; set; }

    [Required]
    [RegularExpression(@"^\d+(.\d{1,2})?$", ErrorMessage = "Invalid value")]
    [Range(0.01, int.MaxValue, ErrorMessage = "The Amount must be greater than $0.00")]
    public decimal Amount { get; set; }

    public ulong TransactionId { get; set; }
    // For validation only, not part of the schema - start
    public ExpenseCategoryModel ExpenseCategoryModel { get; set; } = new();
    public BankModel BankModel { get; set; } = new();
    // For validation only, not part of the schema - end
#nullable enable

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        List<ValidationResult> results = new();

        if (BankModel.CurrentBalance < Amount)
        {
            results.Add(new ValidationResult("Not enough funds.", new[] { nameof(Amount) }));
        }

        return results;
    }
}
