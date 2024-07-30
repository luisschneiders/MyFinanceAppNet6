namespace MyFinanceAppLibrary.Records;

public record BasicAuthenticationData
{
    public string Username  { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
