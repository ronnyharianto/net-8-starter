using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Net;

namespace NET.Starter.Shared.Objects.Dtos
{
    /// <summary>
    /// Represents a paginated response containing a collection of data along with pagination details.
    /// </summary>
    /// <typeparam name="T">The type of items in the paginated collection.</typeparam>
    public class PagingDto<T>(string? message = null, HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest) : BaseDto(message, httpStatusCode)
        where T : class
    {
        /// <summary>
        /// The current page number (starting from 1).
        /// </summary>
        public int Page { get; private set; }

        /// <summary>
        /// The number of records included per page.
        /// </summary>
        public int PageSize { get; private set; }

        /// <summary>
        /// The total number of pages based on the total record count and page size.
        /// </summary>
        public int TotalPage { get; private set; }

        /// <summary>
        /// The total number of records available after filtering.
        /// </summary>
        public int RecordsFiltered { get; private set; }

        /// <summary>
        /// The number of records in the current page.
        /// </summary>
        public int PageRecordCount => Obj?.Count() ?? 0;

        /// <summary>
        /// Indicates whether there is a next page after the current page.
        /// </summary>
        public bool HasNext => Page < TotalPage && TotalPage > 1;

        /// <summary>
        /// Indicates whether there is a previous page before the current page.
        /// </summary>
        public bool HasPrevious => Page > 1;

        /// <summary>
        /// The collection of data items for the current page.
        /// </summary>
        public IEnumerable<T>? Obj { get; private set; }

        /// <summary>
        /// Calculates how many pages are needed to display all records.
        /// </summary>
        /// <param name="totalData">The total number of records.</param>
        /// <param name="pageSize">The number of records shown per page.</param>
        /// <returns>The total number of pages.</returns>
        private static int CalculateTotalPage(int totalData, int pageSize) 
            => pageSize > 0 ? (totalData + pageSize - 1) / pageSize : 0;

        /// <summary>
        /// Applies pagination on a queryable data source, setting paging properties accordingly.
        /// </summary>
        /// <param name="page">Requested page number (1-based).</param>
        /// <param name="pageSize">Number of records per page.</param>
        /// <param name="obj">The source data query to paginate.</param>
        /// <param name="message">Optional message indicating success or additional info.</param>
        public async Task ApplyPagination(int page, int pageSize, IQueryable<T>? obj, string? message = null)
        {
            Page = page > 0 ? page : 1;
            PageSize = pageSize > 0 ? pageSize : 0;

            if (obj == null)
            {
                RecordsFiltered = 0;
                Obj = [];
            }
            else
            {
                RecordsFiltered = obj.Provider is IAsyncQueryProvider ? await obj.CountAsync() : obj.Count();
                Obj = obj.Skip((Page - 1) * PageSize).Take(PageSize);
            }

            TotalPage = CalculateTotalPage(RecordsFiltered, PageSize);

            MarkAsSuccess(message);
        }

        /// <summary>
        /// Copies the pagination details from another paging DTO and replaces the current data collection.
        /// 
        /// This is useful when some data processing cannot be performed in a queryable context (like complex projections)
        /// and you need to paginate first, then replace the data with a different collection.
        /// </summary>
        /// <typeparam name="TSource">The source type of the paging DTO to copy from.</typeparam>
        /// <param name="pagingDto">The source paging DTO to copy pagination metadata from.</param>
        /// <param name="obj">The new collection of data items to set.</param>
        public void CopyPagination<TSource>(PagingDto<TSource> pagingDto, IEnumerable<T>? obj)
            where TSource : class
        {
            Page = pagingDto.Page;
            PageSize = pagingDto.PageSize;
            TotalPage = pagingDto.TotalPage;
            RecordsFiltered = pagingDto.RecordsFiltered;
            Obj = obj;

            MarkAsSuccess(pagingDto.Message);
        }
    }
}