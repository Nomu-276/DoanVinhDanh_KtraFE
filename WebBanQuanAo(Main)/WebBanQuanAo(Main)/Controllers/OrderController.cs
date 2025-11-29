using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanQuanAo_Main_.Models;
using WebBanQuanAo_Main_.Models.ViewModel;

namespace WebBanQuanAo_Main_.Controllers
{
    public class OrderController : Controller
    {
        DBClothingStoreEntities db = new DBClothingStoreEntities();
        // GET: Order/Checkout
        
        [Authorize] // Yêu cầu đăng nhập [cite: 419]
        public ActionResult Checkout()
        {
            // 1. Kiểm tra giỏ hàng trong session [cite: 416]
            var cart = Session["Cart"] as List<CartItem>;
            if (cart == null || !cart.Any())
            {
                return RedirectToAction("Index", "Home"); // Giỏ hàng rỗng thì về trang chủ [cite: 418]
            }

            // 2. Lấy thông tin người dùng hiện tại [cite: 420]
            var user = db.AdminUsers.SingleOrDefault(u => u.NameUser == User.Identity.Name);
            if (user == null) return RedirectToAction("Login", "Account");

            // 3. Lấy thông tin khách hàng tương ứng [cite: 423]
            var customer = db.Customers.SingleOrDefault(c => c.NameUser == user.NameUser);
            if (customer == null) return RedirectToAction("UpdateProfile", "Account");

            // 4. Tạo ViewModel để hiển thị [cite: 424]
            var model = new CheckoutVM
            {
                CartItems = cart, // Danh sách sản phẩm để hiển thị bên cột phải
                TotalAmount = cart.Sum(item => item.total), // Tổng tiền
                OrderDate = DateTime.Now,
                //ShippingAddress = customer.CustomerAddress, // Lấy địa chỉ mặc định
                CustomerID = customer.IDCus,
                Username = customer.NameUser
            };

            return View(model);
        }

        // POST: Order/Checkout
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Checkout(CheckoutVM model)
        {
            var cart = Session["Cart"] as List<CartItem>;
            if (cart == null || !cart.Any()) return RedirectToAction("Index", "Home");

            if (ModelState.IsValid)
            {
                // 1. Khởi tạo đơn hàng (OrderPro) [cite: 429]
                // Lưu ý: Mapping từ CheckoutVM sang OrderPro
                var order = new OrderPro
                {
                    IDCus = model.CustomerID, // Mapping CustomerID sang IDCus
                    DateOrder = DateTime.Now,
                    TotalAmount = model.TotalAmount,
                    PaymentStatus = "Chờ xử lý", // Mặc định [cite: 426]
                    PaymentMethod = model.PaymentMethod,
                    
                    ShippingAddress = model.ShippingAddress
                };

                // 2. Thêm đơn hàng vào DB [cite: 432]
                db.OrderProes.Add(order);
                db.SaveChanges(); // Lưu để lấy được Order ID

                // 3. Tạo chi tiết đơn hàng (OrderDetail) [cite: 430]
                foreach (var item in cart)
                {
                    var orderDetail = new OrderDetail
                    {
                        IDOrder = order.ID,
                        ProductID = item.idPro,
                        Quantity = item.quantity,
                        UnitPrice = item.Price,
                        TotalPrice = item.total, // Cột này trong DB nên là computed column như slide[cite: 406], nhưng nếu lưu cứng thì gán ở đây
                    };
                    db.OrderDetails.Add(orderDetail);
                }
                db.SaveChanges(); // Lưu chi tiết đơn hàng [cite: 433]

                // 4. Xóa giỏ hàng sau khi đặt thành công [cite: 435]
                Session["Cart"] = null;

                // 5. Chuyển hướng sang trang thông báo thành công
                return RedirectToAction("OrderSuccess", new { id = order.ID });
            }

            // Nếu model lỗi, load lại view kèm cart
            model.CartItems = cart;
            return View(model);
        }

        // Action xác nhận đơn hàng [cite: 450]
        public ActionResult OrderSuccess(int id)
        {
            var order = db.OrderProes.Find(id);
            return View(order);
        }
    }
}