using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBanQuanAo_Main_.Models.ViewModel
{
    public class HomeProductVM
    {
        // tiêu chí để search theo tên, mô tả sản phẩm
        //hoặc loại sản phẩm
        public string SearchTerm { get; set; }
        // các thuộc tính hỗ trợ phân trang
        public int PageNumber { get; set; } //trang hiện tại
        public int PageSize { get; set; } = 10;//số sản phẩm trên mỗi trang
        //danh sách sản phẩm  nổi bật
        public List<Product> FeaturedProducts { get; set; }
        //Danh sách sản phẩm mới đã phân trang
        
    }
}