namespace auth0rize.auth.domain.Primitives
{
    public class FilterOption
    {
        public object Value { get; set; }
        public FilterOperator Operator { get; set; } = FilterOperator.Equals;
    }
    public enum FilterOperator
    {
        Equals,
        StartsWith,
        EndsWith,
        Contains,
        IsNull,
        IsNotNull
    }
}
