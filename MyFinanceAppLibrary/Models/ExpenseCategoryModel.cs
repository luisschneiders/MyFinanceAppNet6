using System.ComponentModel.DataAnnotations;

namespace MyFinanceAppLibrary.Models;

public class ExpenseCategoryModel : BaseModel
{
#nullable disable
    public ulong Id { get; set; }
    [Required]
    public string Description { get; set; }
    public string Color { get; set; }
#nullable enable
}
