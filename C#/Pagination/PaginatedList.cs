using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Framework.Pagination
{
    public class PaginatedList<T>
    {
        public IEnumerable<T> Source { get; set; }

        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }

        public bool HasPrevious { get; set; }
        public bool HasNext { get; set; }
    }
}