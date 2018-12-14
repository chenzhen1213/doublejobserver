using System;
using System.Collections.Generic;
using System.Text;

//创建一个类可以存放翻页的数据

namespace DoubleJobServer.Core.DomainModels
{
    //该类继承于List<T>，同时还包含PaginationBase作为属性，还可以判断是否有前一页和后一页。使用静态方法创建该类的实例。
    public class PaginatedList<T> : List<T> where T : class
    {
        public PaginationBase PaginationBase { get; }

        public int TotalItemsCount { get; set; }
        public int PageCount => TotalItemsCount / PaginationBase.PageSize + (TotalItemsCount % PaginationBase.PageSize > 0 ? 1 : 0);

        public bool HasPrevious => PaginationBase.PageIndex > 0;
        public bool HasNext => PaginationBase.PageIndex < PageCount - 1;

        public PaginatedList(int pageIndex, int pageSize, int totalItemsCount, IEnumerable<T> data)
        {
            PaginationBase = new PaginationBase
            {
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            TotalItemsCount = totalItemsCount;
            AddRange(data);
        }
    }
}

