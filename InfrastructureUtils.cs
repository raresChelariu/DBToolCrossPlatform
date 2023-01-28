using System.Data;
using System.Text.RegularExpressions;

namespace DBToolCrossPlatform;

public class InfrastructureUtils
{
    public static readonly Regex RegexSplitLines = new("\r\n|\r|\n|\t", RegexOptions.Compiled);

    public static DataTable FromCsvToDataTable(string csvString, char separator)
    {
        var result = new DataTable();

        var csvStringLines = RegexSplitLines.Split(csvString);

        var headerColumns = csvStringLines[0].Split(separator);
        headerColumns.ToList().ForEach(header =>
        {
            result.Columns.Add(header, typeof(string));
        });

        for (var i = 1; i < csvStringLines.Length; i++)
        {
            var tableRow = result.NewRow();
            var colValues = csvStringLines[i].Split(separator);
            if (colValues.Length != headerColumns.Length)
                return null;
            for (var j = 0; j < colValues.Length; j++)
            {
                tableRow.SetField(j, colValues[j].Trim());
            }
            result.Rows.Add(tableRow);
        }
        return result;
    }

    public static async Task<string> GetFileAsString(OpenFileDialog ofd)
    {
        var fileStream = ofd.OpenFile();
        var reader = new StreamReader(fileStream);
        try
        {
            var fileContent = await reader.ReadToEndAsync();
            return fileContent;
        }
        catch (Exception)
        {
            return null;
        }
    }



}
