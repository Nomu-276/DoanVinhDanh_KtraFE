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
        

        // Helper để lấy Cart từ Session
        private Cart GetCart()
        {
            var cart = Session["Cart"] as Cart;
            if (cart == null || cart.Items.Count() == 0)
            {
                return null;
            }
            return cart;
        }

        // GET: Order/Checkout
        [Authorize]
        public ActionResult Checkout()
        {
            // 1. Kiểm tra giỏ hàng
            var cart = GetCart();
            if (cart == null) return RedirectToAction("Index", "Cart");

            // 2. Lấy thông tin khách hàng (Dựa trên Identity Name đang đăng nhập)
            // Lưu ý: Logic này giả định NameUser trong AdminUser khớp với NameUser trong Customer
            var user = db.AdminUsers.SingleOrDefault(u => u.NameUser == User.Identity.Name);
            if (user == null) return RedirectToAction("Login", "Account");

            var customer = db.Customers.SingleOrDefault(c => c.NameUser == user.NameUser);
            if (customer == null) return RedirectToAction("UpdateProfile", "Account"); // Chưa có info khách hàng thì đi cập nhật

            // 3. Tạo ViewModel
            var model = new CheckoutVM
            {
                CartItems = cart.Items.ToList(),
                TotalAmount = cart.TotalValue(),
                OrderDate = DateTime.Now,
                ShippingAddress = customer.AddressCus, // Lấy địa chỉ mặc định từ bảng Customer
                CustomerID = customer.IDCus,
                Username = customer.NameUser,
                PaymentMethod = "Tiền mặt" // Giá trị mặc định
            };

            return View(model);
        }

        // POST: Order/Checkout
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Checkout(CheckoutVM model)
        {
            var cart = GetCart();
            if (cart == null) return RedirectToAction("Index", "Cart");

            if (ModelState.IsValid)
            {
                // 1. Tạo đơn hàng (OrderPro)
                var order = new OrderPro
                {
                    IDCus = model.CustomerID,
                    DateOrder = DateTime.Now,
                    TotalAmount = cart.TotalValue(),
                    PaymentStatus = "Chờ xử lý",
                    PaymentMethod = model.PaymentMethod,
                    DeliveryMethod = "Giao hàng nhanh", // Có thể mở rộng form để chọn
                    ShippingAddress = model.ShippingAddress
                };

                db.OrderProes.Add(order);
                db.SaveChanges(); // Lưu để sinh IDOrder

                // 2. Tạo chi tiết đơn hàng (OrderDetail)
                foreach (var item in cart.Items)
                {
                    var orderDetail = new OrderDetail
                    {
                        IDOrder = order.ID,
                        ProductID = item.idPro,
                        Quantity = item.quantity,
                        UnitPrice = item.Price,
                        TotalPrice = item.total
                    };
                    db.OrderDetails.Add(orderDetail);
                }

                // Trừ tồn kho (Optional - logic nâng cao)
                // foreach (var item in cart.Items) { var p = db.Products.Find(item.idPro); p.RemainQuantity -= item.quantity; ... }

                db.SaveChanges();

                // 3. Xóa giỏ hàng
                Session["Cart"] = null;

                // 4. Chuyển hướng
                return RedirectToAction("OrderSuccess", new { id = order.ID });
            }

            // Nếu model lỗi, cần gán lại CartItems để view không bị null reference
            model.CartItems = cart.Items.ToList();
            return View(model);
        }

        // GET: Order/OrderSuccess
        // Bạn cần thêm using này ở đầu file để dùng được .Include

        public ActionResult OrderSuccess(int id)
        {
            var order = db.OrderProes
                          .Include("OrderDetails")
                          .Include("OrderDetails.Product")
                          .FirstOrDefault(o => o.ID == id);

            if (order == null)
            {
                return HttpNotFound();
            }

            return View(order);
        }

    // GET: Order/History (Lịch sử mua hàng)
    [Authorize]
        public ActionResult History()
        {
            // Lấy ID khách hàng hiện tại
            var user = db.AdminUsers.SingleOrDefault(u => u.NameUser == User.Identity.Name);
            var customer = db.Customers.SingleOrDefault(c => c.NameUser == user.NameUser);

            if (customer == null) return RedirectToAction("Index", "Home");

            // Lấy danh sách đơn hàng giảm dần theo ngày
            var orders = db.OrderProes
                           .Where(o => o.IDCus == customer.IDCus)
                           .OrderByDescending(o => o.DateOrder)
                           .ToList();

            return View(orders);
        }
    }
}