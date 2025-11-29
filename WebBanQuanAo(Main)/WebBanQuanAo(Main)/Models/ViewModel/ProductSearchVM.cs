using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList.Mvc;
namespace WebBanQuanAo_Main_.Models.ViewModel
{
    public class ProductSearchVM
    {
        public string SearchTerm { get; set; }
        // search theo giá
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        //Thứ tự sắp xếp
        public string SortOrder { get; set; }
        //Các thuộc tính hỗ trợ phân trang
        public int PageNumber { get; set; }// Trang hiện tại
        public int PageSize { get; set; } = 10; //Số sản phẩm trên mỗi trang
        //Danh sách sản phẩm đã phân trang
        public PagedList.IPagedList<Product> Products { get; set; }
        //public List<Product> Products { get; set; }

    }
}