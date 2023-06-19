using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.Pagination
{
    public static class Extensions 
    {
        public static ReadOnlyPaginatedList<T> ToPaginatedList<T>(this IEnumerable<T> input, int count, int currentPageNumber,
            int pageSize)
        {
            return new ReadOnlyPaginatedList<T>(input, count, currentPageNumber, pageSize);
        }

        public static Dictionary<string, string> ToParametersDictionary(this QueryStringParameters parameters)
        {
            return new Dictionary<string, string>
            {
                {nameof(QueryStringParameters.PageNumber), parameters.PageNumber.ToString() },
                {nameof(QueryStringParameters.PageSize), parameters.PageSize.ToString() },
            };
        }

        public static bool IsDefault(this QueryStringParameters parameters) =>
            parameters.PageNumber == QueryStringParameters.DefaultPageNumber && parameters.PageSize == QueryStringParameters.DefaultPageSize;
    }
}
