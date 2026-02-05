namespace ExtensionsLibrary;

public static class StringExtensions
{
    public static string Truncate(this string value, int maxLength, string suffix = "...")
    {
        if (string.IsNullOrEmpty(value))
            return value;

        return value.Length > maxLength
            ? value[..maxLength] + suffix
            : value;
    }

    public static string EscapeCsv(this string value)
    {
        if (string.IsNullOrEmpty(value))
            return string.Empty;

        var escaped = value.Replace("\"", "\"\"");
        return $"\"{escaped}\"";
    }
}