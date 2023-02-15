using System.ComponentModel.DataAnnotations;

namespace MyFinanceAppLibrary.Models;

public class TransactionModel : BaseModel, IValidatableObject
{
#nullable disable
    public ulong Id { get; set; }

    public ulong Link { get; set; }

    [Required]
    public DateTime TDate { get; set; }

    public TransactionCategoryModel TCategoryType { get; set; } = new();

    public BankModel FromBank { get; set; } = new();

    public BankModel ToBank { get; set; } = new();

    //[Required]
    //public string Action { get; set; }

    //[Required]
    //public string Label { get; set; }

    [Required]
    public string Comments { get; set; }

    [Required]
    [RegularExpression(@"^\d+(.\d{1,2})?$", ErrorMessage = "Invalid value")]
    [Range(0.01, int.MaxValue, ErrorMessage = "The Amount must be greater than $0.00.")]
    public decimal Amount { get; set; }

#nullable enable

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        List<ValidationResult> results = new();

        if (TCategoryType.Id <= 0)
        {
            results.Add(new ValidationResult("The Action field is required.", new[] { nameof(TCategoryType.Id) }));
        }

        if (FromBank.Id <= 0)
        {
            results.Add(new ValidationResult("The From Bank field is required.", new[] { nameof(FromBank.Id) }));
        }

        if (TCategoryType.ActionType == "T" || TCategoryType.ActionType == "D")
        {
            if (FromBank.CurrentBalance < Amount)
            {
                results.Add(new ValidationResult("Not enough funds.", new[] { nameof(Amount) }));
            }
        }

        if (TCategoryType.ActionType == "T")
        {
            if (ToBank.Id <=0 )
            {
                results.Add(new ValidationResult("The To Bank field is required.", new[] { nameof(ToBank.Id) }));
            }
            if (FromBank.Id == ToBank.Id)
            {
                results.Add(new ValidationResult("From Bank and To Bank can not be the same.", new[] { nameof(ToBank.Id) }));
            }
        }

        return results;
    }
}
