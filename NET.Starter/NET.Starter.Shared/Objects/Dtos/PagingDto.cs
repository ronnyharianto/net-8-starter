using NET.Starter.Shared.Enums;

namespace NET.Starter.Shared.Objects.Dtos
{
    /// <summary>
    /// Represents a paginated response containing a data payload.
    /// </summary>
    /// <typeparam name="T">The type of the paginated data.</typeparam>
    public class PagingDto<T>(string? message = null, ResponseCode responseCode = ResponseCode.BadRequest) : BaseDto(message, responseCode)
        where T : class
    {
        /// <summary> Current page number. </summary>
        public int Page { get; private set; }

        /// <summary> Number of records per page. </summary>
        public int PageSize { get; private set; }

        /// <summary> The total number of pages required to display all records. </summary>
        public int TotalPage { get; private set; }

        /// <summary> The total number of records before applying any filters. </summary>
        public int RecordsTotal { get; private set; }

        /// <summary> The total number of records that match the filter criteria. </summary>
        public int RecordsFiltered { get; private set; }

        /// <summary> Indicates if there is a next page available. </summary>
        public bool HasNext => Page < TotalPage && TotalPage > 1;

        /// <summary> Indicates if there is a previous page available. </summary>
        public bool HasPrevious => Page > 1;

        /// <summary> The collection of data items for the current page. </summary>
        public IEnumerable<T>? Obj { get; private set; }

        /// <summary>
        /// Calculates the total number of pages required to display all data.
        /// </summary>
        /// <param name="totalData">The total number of records.</param>
        /// <param name="pageSize">The number of records per page.</param>
        /// <returns>The total number of pages.</returns>
        private static int CalculateTotalPage(int totalData, int pageSize) => (totalData + pageSize - 1) / pageSize;

        /// <summary>
        /// Applies pagination to the given data source and updates the pagination properties.
        /// </summary>
        /// <param name="page">Current page number.</param>
        /// <param name="pageSize">Number of records per page.</param>
        /// <param name="obj">Queryable data source.</param>
        /// <param name="message">Optional success message.</param>
        public void ApplyPagination(int page, int pageSize, IQueryable<T>? obj, string? message = null)
        {
            Page = page;
            PageSize = pageSize;
            RecordsFiltered = obj?.Count() ?? 0;
            RecordsTotal = obj?.Count() ?? 0;
            TotalPage = CalculateTotalPage(RecordsTotal, PageSize);
            Obj = obj?.Skip((page - 1) * pageSize).Take(pageSize);

            MarkAsSuccess(message);
        }
    }
}