namespace NET.Starter.Shared.Objects.Inputs.Interfaces
{
    /// <summary>
    /// Defines input for keyword-based filtering or search operations.
    /// </summary>
    public interface ISearchInput
    {
        /// <summary>
        /// The keyword used to filter or search data.
        /// If left empty or null, no search filtering will be applied.
        /// </summary>
        string? SearchKey { get; set; }
    }
}
