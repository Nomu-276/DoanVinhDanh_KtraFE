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
        public string image { get; set; }

        public string color { get; set; }
        public string size { get; set; }

        public int quantity { get; set; }
        public decimal price { get; set; }

        public decimal total => quantity * price;
    }
}