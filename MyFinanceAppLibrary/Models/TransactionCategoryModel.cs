using System.ComponentModel.DataAnnotations;

namespace MyFinanceAppLibrary.Models;

public class TransactionCategoryModel : BaseModel
{
#nullable disable
    public ulong Id { get; set; }

    [Required]
    public string Description { get; set; }

    [Required]
    public string ActionType { get; set; }
#nullable enable    
}
