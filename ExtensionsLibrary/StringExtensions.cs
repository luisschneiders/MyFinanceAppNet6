namespace ExtensionsLibrary;

public static class StringExtensions
{
    public static string? Truncate(this string? value, int maxLength, string suffix = "...")
    {
        return value?.Length > maxLength ? value.Substring(0, maxLength) + suffix : value;
    }
}
