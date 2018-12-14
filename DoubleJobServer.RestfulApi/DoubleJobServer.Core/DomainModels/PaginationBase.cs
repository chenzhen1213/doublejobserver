using System;
using System.Collections.Generic;
using System.Text;
using DoubleJobServer.Core.Interfaces;

namespace DoubleJobServer.Core.DomainModels
{
    public class PaginationBase
    {
        private int _pageSize = 10;
        public int PageIndex { get; set; } = 0;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
        }

        //OrderBy属性，默认值是“Id”，因为翻页必须要先排序，但目前这个OrderBy属性还没用上
        public string OrderBy { get; set; } = nameof(IEntity.Id);
        public string Fields { get; set; }
        protected int MaxPageSize { get; set; } = 100;
    }
}
