using System;
using System.Collections.Generic;
using System.Linq;

namespace Framework.Pagination
{
    public class ReadOnlyPaginatedList<T>
    {
        public IReadOnlyCollection<T> Source { get; }

        public int CurrentPage { get; }
        public int TotalPages { get; }
        public int PageSize { get; }
        public int TotalCount { get; }

        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;

        public ReadOnlyPaginatedList(IEnumerable<T> items, int totalCount, int currentPageNumber, int pageSize)
        {
            TotalCount = totalCount;
            PageSize = pageSize;
            CurrentPage = currentPageNumber;
            TotalPages = (int) Math.Ceiling(totalCount / (double) pageSize);

            Source = items.ToList();
        }
    }
}