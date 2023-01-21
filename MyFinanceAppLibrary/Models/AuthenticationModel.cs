using System.ComponentModel.DataAnnotations;

namespace MyFinanceAppLibrary.Models;

public class AuthenticationModel
{
#nullable disable
    [Required]
    public string Username { get; set; }

    [Required]
    public string Password { get; set; }
#nullable enable
}
