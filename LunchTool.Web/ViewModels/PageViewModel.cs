using System;
using System.Collections.Generic;

namespace LunchTool.Web.ViewModels
{
    public class PageViewModel<T> where T: class
    {
        public T Item { get; set; }
        public PageInfo Info { get; }

        public PageViewModel(T item, int count, int pageNumber, int pageSize)
        {
            Item = item;
            Info = new PageInfo(count, pageNumber, pageSize);
        }

        public class PageInfo
        {
            public int PageNumber { get; set; }
            public int TotalPages { get; set; }

            public PageInfo(int count, int pageNumber, int pageSize)
            {
                PageNumber = pageNumber;
                TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            }

            public bool HasPreviousPage
            {
                get => PageNumber > 1;
            }

            public bool HasNextPage
            {
                get => PageNumber < TotalPages;
            }
        }
    }
}
