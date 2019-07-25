using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutoFacMvc.Models.ViewModels
{
    public class PageViewModel
    {
        public PageViewModel()
        {

        }

        public PageViewModel(int currentIndex, int totalCount)
        {
            TotalCount = totalCount;
            CurrentIndex = currentIndex;
        }

        /// <summary>
        ///点击分页是指向 Action 的名字 根据具体需要而定
        /// </summary>
        public virtual string ActionName => "Index";

        /// <summary>
        /// 根据需要具体而定 分页默认页码值
        /// </summary>
        public virtual int PageSize => 12;

        /// <summary>
        ///根据需要具体而定 分页显示最大的页数 
        /// </summary>
        public virtual int DisplayMaxPages => 10;

        /// <summary>
        /// 总记录数
        /// </summary>
        public int TotalCount { get; set; } = 0;

        /// <summary>
        /// 当前页面值
        /// </summary>
        public int CurrentIndex { get; set; } = 1;

        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / (double)PageSize);

        /// <summary>
        /// 是否有上一页
        /// </summary>
        public bool IsHasPrePage => CurrentIndex != 1;

        /// <summary>
        /// 是否有下一页
        /// </summary>
        public bool IsHasNextPage => CurrentIndex != TotalPages;
    }
}