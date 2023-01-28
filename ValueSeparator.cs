namespace DBToolCrossPlatform;

internal class ValueSeparator
{

    private static readonly Dictionary<string, char> ValueSeparators = new()
    {
        {"Comma", ','},
        {"Space", ' ' }
    };

    public static bool TryGetSeparatorFromName(string name, out char separator)
        => ValueSeparators.TryGetValue(name, out separator);

    public readonly char Value;

    public ValueSeparator(ComboBox uiValueSource)
    {
        var selectedIndex = uiValueSource.SelectedIndex;
        var separatorName = uiValueSource.Items[selectedIndex].ToString();

        var isImplemented = TryGetSeparatorFromName(separatorName, out var separator);
        if (!isImplemented)
        {
            throw new NotImplementedException($"{nameof(separatorName)}:{separatorName}");
        }

        Value = separator;
    }
}