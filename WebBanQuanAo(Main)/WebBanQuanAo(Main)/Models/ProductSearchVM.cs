using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBanQuanAo_Main_.Models
{
    public class ProductSearchVM
    {
        // Tiêu chí tìm kiếm
        public string SearchTerm { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }

        // Thứ tự sắp xếp
        public string SortOrder { get; set; }

        // Thuộc tính hỗ trợ phân trang
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 2; // Ví dụ: 2 sản phẩm mỗi trang

        
    }
}