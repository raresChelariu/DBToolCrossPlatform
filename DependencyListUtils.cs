using System.Data;

namespace DBToolCrossPlatform;

internal class DependencyListUtils
{
    public static bool PairListStringEquality(List<string> a, List<string> b)
    {
        if (a.Count != b.Count)
            return false;
        return !a.Where((t, i) => !t.Equals(b[i])).Any();
    }

    public static bool ListStringEquality(params List<string>[] lists)
    {
        for (var i = 1; i < lists.Length; i++)
        {
            if (false == PairListStringEquality(lists[i - 1], lists[i]))
                return false;
        }
        return true;
    }

    public static List<string> GetSubsetValuesFromRow(List<int> set, DataRow row)
    { 
        var rowValues = row.ItemArray.ToList().Select(x => x.ToString()).ToList();

        return set.Select(t => rowValues[t]).ToList();
    }

    public static List<Dependency> GetAllDependenciesFromSubsetList(List<List<int>> subsetList)
    {
        var result = new List<Dependency>();
        for (var i = 0; i < subsetList.Count; i++)
        for (var j = 0; j < subsetList.Count; j++)
        {
            if (i == j) continue;
            var hasAtLeastOneCommon = subsetList[i].Intersect(subsetList[j]).Any();
            if (!hasAtLeastOneCommon)
                result.Add(new Dependency(subsetList[i], subsetList[j]));
        }
        return result;
    }

    public static List<List<int>> GetAllSubsets(int setSize)
    {
        var isLeft = new bool[setSize];

        var sets = new List<List<int>>();

        long maxNoSets = 1 << setSize;
        for (long currentSetIteration = 1; currentSetIteration < maxNoSets; currentSetIteration++)
        {
            var currentSet = new List<int>();

            for (var i = 0; i < setSize; i++)
            {
                isLeft[i] = (currentSetIteration & (1 << i)) != 0;
            }
            for (var i = 0; i < setSize; i++)
            {
                if (isLeft[i])
                {
                    currentSet.Add(i);
                }
            }
            sets.Add(currentSet);
        }
        return sets;
    }

}