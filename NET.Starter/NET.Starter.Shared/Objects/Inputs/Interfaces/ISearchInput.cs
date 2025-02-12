namespace NET.Starter.Shared.Objects.Inputs.Interfaces
{
    /// <summary>
    /// Represents a search input for filtering data.
    /// </summary>
    public interface ISearchInput
    {
        /// <summary>
        /// Gets or sets the search keyword.
        /// If null or empty, no filtering will be applied.
        /// </summary>
        public string? SearchKey { get; set; }
    }
}