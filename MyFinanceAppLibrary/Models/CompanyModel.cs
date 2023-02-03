using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MyFinanceAppLibrary.Constants;

namespace MyFinanceAppLibrary.Models;

public class CompanyModel : BaseModel
{
#nullable disable
    public ulong Id { get; set; }

    [Required]
    public string Description { get; set; }

    [Required]
    [RegularExpression(@"^\d+(.\d{1,2})?$", ErrorMessage = "Invalid value")]
    public decimal Rate { get; set; }

    [Required]
    [Range(1, 3, ErrorMessage = "The Company Type field is required.")]
    public int CType { get; set; }
#nullable enable
}
