using System.ComponentModel.DataAnnotations;

namespace MyFinanceAppLibrary.Models;

public class TransactionModel : BaseModel, IValidatableObject
{
#nullable disable
    public ulong Id { get; set; }

    public ulong Link { get; set; }

    [Required]
    public DateTime TDate { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "The Action field is required.")]
    public ulong TCategoryType { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "The From Bank field is required.")]
    public ulong FromBank { get; set; }

    // For validation only, not part of the schema - start
    public ulong ToBank { get; set; }
    public TransactionCategoryModel TCategoryTypeModel { get; set; } = new();
    public BankModel FromBankModel { get; set; } = new();
    public BankModel ToBankModel { get; set; } = new();
    // For validation only, not part of the schema - end

    public string Action { get; set; }
    public string Label { get; set; }

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

        if (TCategoryTypeModel.ActionType == TransactionActionType.T.ToString() ||
            TCategoryTypeModel.ActionType == TransactionActionType.D.ToString())
        {
            if (FromBankModel.CurrentBalance < Amount)
            {
                results.Add(new ValidationResult("Not enough funds.", new[] { nameof(Amount) }));
            }
        }

        if (TCategoryTypeModel.ActionType == TransactionActionType.T.ToString())
        {
            if (ToBankModel.Id <= 0)
            {
                results.Add(new ValidationResult("The To Bank field is required.", new[] { nameof(ToBank) }));
            }
            if (FromBankModel.Id == ToBankModel.Id)
            {
                results.Add(new ValidationResult("From Bank and To Bank can not be the same.", new[] { nameof(ToBank) }));
            }
        }

        return results;
    }
}
