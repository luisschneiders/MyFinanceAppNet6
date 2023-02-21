using System.ComponentModel.DataAnnotations;

namespace MyFinanceAppLibrary.Models;

public class ExpenseModel : BaseModel, IValidatableObject
{
#nullable disable
    public ulong Id { get; set; }

    [Required]
    public DateTime EDate { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "The Expense field is required.")]
    public ulong ExpenseCategoryId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "The Bank field is required.")]
    public ulong BankId { get; set; }

    public ulong TransactionId { get; set; }

    [Required]
    public string Comments { get; set; }

    [Required]
    [RegularExpression(@"^\d+(.\d{1,2})?$", ErrorMessage = "Invalid value")]
    [Range(0.01, int.MaxValue, ErrorMessage = "The Amount must be greater than $0.00.")]
    public decimal Amount { get; set; }
#nullable enable

    // For validation only, not part of the schema - start
    public ExpenseCategoryModel ECategoryTypeModel { get; set; } = new();
    public BankModel BankModel { get; set; } = new();
    // For validation only, not part of the schema - end

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        List<ValidationResult> results = new();

        //if (TCategoryTypeModel.ActionType == TransactionActionType.T.ToString() ||
        //    TCategoryTypeModel.ActionType == TransactionActionType.D.ToString())
        //{
        //    if (FromBankModel.CurrentBalance < Amount)
        //    {
        //        results.Add(new ValidationResult("Not enough funds.", new[] { nameof(Amount) }));
        //    }
        //}

        //if (TCategoryTypeModel.ActionType == TransactionActionType.T.ToString())
        //{
        //    if (ToBankModel.Id <= 0)
        //    {
        //        results.Add(new ValidationResult("The To Bank field is required.", new[] { nameof(ToBank) }));
        //    }
        //    if (FromBankModel.Id == ToBankModel.Id)
        //    {
        //        results.Add(new ValidationResult("From Bank and To Bank can not be the same.", new[] { nameof(ToBank) }));
        //    }
        //}

        return results;
    }
}
