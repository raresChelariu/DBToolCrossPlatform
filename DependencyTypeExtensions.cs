namespace DBToolCrossPlatform;

internal static class DependencyTypeExtensions
{
    public static string ToArrow(this DependencyType @this)
        => @this switch
        {
            DependencyType.Functional => "->",
            DependencyType.Multivalued => "->>",
            _ => throw new NotImplementedException(@this.ToString())
        };
}
