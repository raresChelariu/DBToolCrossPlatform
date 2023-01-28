using System.Data;

namespace DBToolCrossPlatform
{
    public class TupleValues
    {
        public List<string> X;
        public List<string> Y;
        public List<string> Z;
        public int Index;

        public TupleValues(Dependency dependency, DataTable table, int index)
        {
            Index = index;
            var row = table.Rows[index];
            X = DependencyListUtils.GetSubsetValuesFromRow(dependency.X, row);
            Y = DependencyListUtils.GetSubsetValuesFromRow(dependency.Y, row);
            if (dependency.Type == DependencyType.Multivalued)
            {
                Z = DependencyListUtils.GetSubsetValuesFromRow(dependency.Z, row);
            }
        }
    }
}
