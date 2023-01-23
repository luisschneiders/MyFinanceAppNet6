namespace MyFinanceAppLibrary.Models;

public class BaseModel
{
#nullable disable
    public bool IsActive { get; set; }
    public bool IsArchived { get; set; }
    public string UpdatedBy { get; set; }
    public DateTime CreateAt { get; set; } = DateTime.Now;
    public DateTime UpdateAt { get; set; } = DateTime.Now;
#nullable enable
}
