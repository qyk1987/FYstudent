using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FYstudentMgr.ViewModels
{
    public class PageResult<T>
    {
        public List<T> Data { get; set; }
        public int Count { get; set; }
        public int CurrentPage { get; set; }
        public string Order { get; set; }
        public bool IsAsc { get; set; }
        public int PageSize { get; set; }
    }
    
}