namespace MyFinanceAppLibrary.Models;

public class BaseModel
{
#nullable disable
    public bool IsActive { get; set; } = true;
    public bool IsArchived { get; set; } = false;
    public string UpdatedBy { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
#nullable enable
}
