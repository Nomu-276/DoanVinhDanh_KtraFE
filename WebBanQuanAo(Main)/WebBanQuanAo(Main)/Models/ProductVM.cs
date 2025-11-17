using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBanQuanAo_Main_.Models
{
    public class ProductVM
    {
        public int IDPro { get; set; }
        public string NamePro { get; set; }
        public string ImagePro { get; set; }
        public Product Product { get; set; }

        
    }
}