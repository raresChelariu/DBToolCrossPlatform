using System.Data;

namespace DBToolCrossPlatform;

public class Dependency : IEqualityComparer<Dependency>, ICloneable
{
    public List<int> X;
    public List<int> Y;
    public List<int> Z;
    public DependencyType Type;
    
    public Dependency(IEnumerable<int> left, IEnumerable<int> right)
    {
        X = new List<int>();
        Y = new List<int>();
        X.AddRange(left);
        Y.AddRange(right);
    }

    public Dependency()
    {

    }

    public string ToDetailedString(List<string> headers, DependencyType dependencyType)
    {
        var leftAttributes = GetAttributesAsString(X, headers);
        var rightAttributes = GetAttributesAsString(Y, headers);
        
        var result = $"{leftAttributes}{dependencyType.ToArrow()}{rightAttributes}";

        return result;
    }

    public bool IsFunctionalDependency(DataTable table)
    {
        ToDetailedString(table.GetColumnNames(), DependencyType.Functional);
        for (var i = 0; i < table.Rows.Count; i++)
        {
            var rowI = table.Rows[i];
            var t1X = DependencyListUtils.GetSubsetValuesFromRow(X, rowI);
            for (var j = i + 1; j < table.Rows.Count; j++)
            {
                var rowJ = table.Rows[j];
                var t2X = DependencyListUtils.GetSubsetValuesFromRow(X, rowJ);

                if (!DependencyListUtils.PairListStringEquality(t1X, t2X))
                    continue;
                var t1Y = DependencyListUtils.GetSubsetValuesFromRow(Y, rowI);
                var t2Y = DependencyListUtils.GetSubsetValuesFromRow(Y, rowJ);
                if (!DependencyListUtils.PairListStringEquality(t1Y, t2Y))
                    return false;

            }
        }
        return true;
    }

    public bool IsMultivaluedDependency(DataTable table)
    {
        Z ??= GetRemainderSetForMultivaluedDependency(this, table.Columns.Count);
        if (Z.Count == 0) 
            return false;
        for (var t1 = 0; t1 < table.Rows.Count; t1++)
        {
            var tuple1 = new TupleValues(this, table, t1);
            for (var t2 = t1 + 1; t2 < table.Rows.Count; t2++)
            {
                var tuple2 = new TupleValues(this, table, t2);
                if (!DependencyListUtils.ListStringEquality(tuple1.X, tuple2.X))
                    continue;
                var existsT3T4ForT1T2Pair = false;
                for (var t3 = 0; t3 < table.Rows.Count && !existsT3T4ForT1T2Pair; t3++)
                {
                    var tuple3 = new TupleValues(this, table, t3);
                    if (!DependencyListUtils.ListStringEquality(tuple2.X, tuple3.X))
                        continue;
                    for (var t4 = 0; t4 < table.Rows.Count && !existsT3T4ForT1T2Pair; t4++)
                    {
                        var tuple4 = new TupleValues(this, table, t4);
                        existsT3T4ForT1T2Pair = DependencyListUtils.ListStringEquality(tuple3.X, tuple4.X) &&
                                                DependencyListUtils.ListStringEquality(tuple1.Y, tuple3.Y) &&
                                                DependencyListUtils.ListStringEquality(tuple2.Y, tuple4.Y) &&
                                                DependencyListUtils.ListStringEquality(tuple2.Z, tuple3.Z) &&
                                                DependencyListUtils.ListStringEquality(tuple1.Z, tuple4.Z);
                    }
                }
                if (!existsT3T4ForT1T2Pair)
                    return false;
            }
        }
        return true;
    }

    public static List<int> GetRemainderSetForMultivaluedDependency(Dependency d, int setSize)
    {
        var unionLeftRight = d.X.Union(d.Y);
        var allColumnsIndexes = new List<int>();
        for (var i = 0; i < setSize; i++)
            allColumnsIndexes.Add(i);
        return allColumnsIndexes.Except(unionLeftRight).ToList();
    }

    public static string GetAttributesAsString(List<int> attributesIndex, List<string> attributesNames)
    {
        return attributesIndex.Aggregate(string.Empty, (result, idx) => result + attributesNames[idx]);
    }

    public static string FromDependencyListToString(List<Dependency> dependencies, DependencyType dependencyType, List<string> attributeNames)
    {
        return string.Join(Environment.NewLine, dependencies.Select(d => d.ToDetailedString(attributeNames, dependencyType)));
    }

    public bool Equals(Dependency x, Dependency y)
    {
        if (x == null || y == null)
            return false;
        return x.X == y.X && x.Y == y.Y && x.Z == y.Z && x.Type == y.Type;
    }

    public int GetHashCode(Dependency obj)
    {
        return HashCode.Combine(obj.X, obj.Y, obj.Z, (int)obj.Type);
    }

    public object Clone() => new Dependency
    {
        X = CloneList(X),
        Y = CloneList(X),
        Z = CloneList(Z),
        Type = Type
    };


    public Dependency CloneWithType(DependencyType type)
    {
        var copy = (Dependency) Clone();
        copy.Type = type;
        return copy;
    }

    private static List<int> CloneList(IEnumerable<int> source)
    {
        if (source == null)
            return null;
        var copy = new List<int>();
        copy.AddRange(source);
        return copy;
    }
}



