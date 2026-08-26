using System.Globalization;

namespace SeventhEditor.Catalog;

public static class FixedPoint
{
    public const int Scale = 65536;

    public static double ToDecimal(int raw) => raw / (double)Scale;

    public static int FromDecimal(double value) => (int)Math.Round(value * Scale);

    public static string Format(int raw)
    {
        var value = ToDecimal(raw);
        return value.ToString("0.####", CultureInfo.CurrentCulture);
    }

    public static bool TryParse(string? text, out int raw)
    {
        raw = 0;
        if (string.IsNullOrWhiteSpace(text))
        {
            return false;
        }

        text = text.Trim();
        if (double.TryParse(text, NumberStyles.Float, CultureInfo.CurrentCulture, out var value) ||
            double.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out value))
        {
            raw = FromDecimal(value);
            return true;
        }

        return false;
    }
}