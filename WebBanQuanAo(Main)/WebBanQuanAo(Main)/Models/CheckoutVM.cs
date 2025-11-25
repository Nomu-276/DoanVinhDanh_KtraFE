using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBanQuanAo_Main_.Models
{
    public class CheckoutVM
    {
        public List<CartItem> CartItems { get; set; }
        public decimal TotalAmount { get; set; }
        // Có thể thêm thông tin Customer mặc định nếu đã đăng nhập
        public Customer CustomerInfo { get; set; }
        // Thêm các thuộc tính cần thiết khác (phí ship, mã giảm giá,...)
    }
}