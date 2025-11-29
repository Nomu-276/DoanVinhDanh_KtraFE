using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBanQuanAo_Main_.Models
{
    public class CartItem
    {
        public int idPro { get; set; }
        public string namePro { get; set; }
        public int quantity { get; set; }
        public decimal Price { get; set; }
        public string image { get; set; }
        public string Size { get; set; }
        public string Color { get; set; }
        public string Category { get; set; }
        // Tổng gia cho moi san pham
        public decimal total => quantity * Price;
    }

}