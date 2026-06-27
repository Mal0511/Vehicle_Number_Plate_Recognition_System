using System.Text;

namespace NumberPlateWeb.Modules.Shared;

public static class PlateNumberFormatter
{
    public static string Normalize(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return string.Empty;
        }

        var builder = new StringBuilder();

        foreach (var character in value.ToUpperInvariant())
        {
            if (char.IsLetterOrDigit(character))
            {
                builder.Append(character);
            }
        }

        return builder.ToString();
    }

    public static string Display(string value)
    {
        var normalized = Normalize(value);

        if (normalized.Length <= 3)
        {
            return normalized;
        }

        return $"{normalized[..3]}-{normalized[3..]}";
    }
}
