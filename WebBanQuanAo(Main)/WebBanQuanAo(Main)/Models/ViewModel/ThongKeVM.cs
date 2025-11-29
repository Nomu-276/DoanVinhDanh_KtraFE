using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBanQuanAo_Main_.Models.ViewModel
{
    public class ThongKeVM
    {
        public string CategoryName { get; set; }
        public int Count { get; set; }
        public decimal MaxPrice { get; set; }
        public decimal MinPrice { get; set; }
        public decimal AvgPrice { get; set; }
    }
}