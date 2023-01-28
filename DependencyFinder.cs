using System.Data;

namespace DBToolCrossPlatform;

internal class DependencyFinder
{
    private readonly DataTable _dataTable;

    private List<string> _attributeNames;

    private List<string> AttributeNames => _attributeNames ??= _dataTable.GetColumnNames();

    public string FunctionalDependenciesAsString;

    public string MultivaluedDependenciesAsString;

    public DependencyFinder(DataTable dataTable)
    {
        _dataTable = dataTable;
    }

    public void Run()
    {
        var attributeNo = _dataTable.Columns.Count;

        var allSubsets = DependencyListUtils.GetAllSubsets(attributeNo);

        var possibleSubsets = DependencyListUtils.GetAllDependenciesFromSubsetList(allSubsets);

        FunctionalDependenciesAsString = GetFunctionalDependenciesFromSubsets(possibleSubsets);

        MultivaluedDependenciesAsString = GetMultivaluedDependenciesFromSubsets(possibleSubsets);
    }

    private string GetMultivaluedDependenciesFromSubsets(IEnumerable<Dependency> subsets)
    {
        var dependenciesMultivalued = subsets
            .Select(x => x.CloneWithType(DependencyType.Multivalued))
            .ToList();

        var validMultivaluedDependencies = dependenciesMultivalued
            .Where(d => d.IsMultivaluedDependency(_dataTable))
            .Distinct()
            .ToList();

        var multivaluedDependenciesToString =
            Dependency.FromDependencyListToString(validMultivaluedDependencies, DependencyType.Multivalued, AttributeNames);

        return multivaluedDependenciesToString;
    }

    private string GetFunctionalDependenciesFromSubsets(IEnumerable<Dependency> subsets)
    {
        var dependenciesFunctional = subsets.Select(x => x.CloneWithType(DependencyType.Functional)).ToList();

        var validFunctionalDependencies = dependenciesFunctional
            .Where(d => d.IsFunctionalDependency(_dataTable))
            .Distinct()
            .ToList();

        var functionalDependenciesToString = Dependency.FromDependencyListToString(validFunctionalDependencies,
            DependencyType.Functional, AttributeNames);

        return functionalDependenciesToString;
    }

    
}