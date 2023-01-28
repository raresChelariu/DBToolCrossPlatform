using System.Data;

namespace DBToolCrossPlatform;

public static class DataTableExtensions
{
    public static List<string> GetColumnNames(this DataTable source)
    {
        return source.Columns.Cast<DataColumn>().Select(column => column.ColumnName).ToList();
    }
}