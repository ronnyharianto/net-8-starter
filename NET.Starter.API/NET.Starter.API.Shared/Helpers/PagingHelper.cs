namespace NET.Starter.API.Shared.Helpers
{
    public static class PagingHelper
    {
        /// <summary>
        /// Calculate total page
        /// </summary>
        public static int CalculateTotalPage(int totalData, int pageSize) => (totalData + pageSize - 1) / pageSize;

        /// <summary>
        /// Implement Skip & Take Data into IQueryable data
        /// </summary>
        public static IQueryable<T> PaginateQuery<T>(this IQueryable<T> data, int page, int pageSize)
            => data.Skip((page - 1) * pageSize).Take(pageSize);
    }
}
