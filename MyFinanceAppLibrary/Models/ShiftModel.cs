using System.ComponentModel.DataAnnotations;

namespace MyFinanceAppLibrary.Models;

public class ShiftModel : BaseModel
{
    public ulong Id { get; set; }

    [Required]
    public DateTime SDate { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Please select a company.")]
    public ulong CompanyId { get; set; }
}
