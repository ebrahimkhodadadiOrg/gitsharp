using System;

namespace Framework.Pagination
{
    public class QueryStringParameters
    {
        public const int MaxPageSize = 100;
        public const int DefaultPageSize = 100;
        public const int DefaultPageNumber = 1;

        public int PageNumber
        {
            get => _pageNumber;
            set
            {
                if (value < 1)
                {
                    throw new Exception("Invalid page number");
                }

                _pageNumber = value;
            }
        }

        private int _pageSize = DefaultPageSize;
        private int _pageNumber = DefaultPageNumber;

        public int PageSize
        {
            get => _pageSize;
            set
            {
                if (value > MaxPageSize || value < 1)
                {
                    throw new Exception("Invalid page size");
                }

                _pageSize = value;
            }
        }

        public int GetSkipper()
        {
            return (PageNumber - 1) * PageSize;
        }
        public int GetTaker()
        {
            return PageNumber * PageSize;
        }
    }
}
