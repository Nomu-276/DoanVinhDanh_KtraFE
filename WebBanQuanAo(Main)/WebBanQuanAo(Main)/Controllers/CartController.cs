using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanQuanAo_Main_.Models;
using WebBanQuanAo_Main_.Models.ViewModel;

namespace WebBanQuanAo_Main_.Controllers
{
    public class CartController : Controller
    {
        private DBClothingStoreEntities db = new DBClothingStoreEntities();
        // Hàm Lấy dịch vụ giỏ hàng
        private CartService GetCartService()
        {
            return new CartService(Session);
        }
        // Hiển thị giỏ hàng đã gom nhóm sản phẩm theo danh mục
        
        // Hàm này nhận tham số page để phân trang sản phẩm gợi ý
        public ActionResult Index(int? page)
        {
            var cart = GetCartService().GetCart();
            var products = db.Products.ToList();
            return View(cart);
        }

        // Thêm sản phẩm vào giỏ
        // Đưa quantity = 1 xuống cuối cùng
        public ActionResult AddToCart(int ProductID, string size, string color, int quantity = 1)
        {
            // Lưu ý: Tôi đã đổi 'int id' thành 'int ProductID' để khớp với name="ProductID" bên file ChiTiet.cshtml
            var product = db.Products.Find(ProductID);
            if (product != null)
            {
                // Xử lý giá trị mặc định nếu khách không chọn size/màu (tránh lỗi null)
                string selectedSize = string.IsNullOrEmpty(size) ? "FreeSize" : size;
                string selectedColor = string.IsNullOrEmpty(color) ? "Mặc định" : color;

                var cartService = GetCartService();
                cartService.GetCart().AddItem(
                    product.ProductID,
                    product.ImagePro,
                    product.NamePro,
                    product.Price ?? 0,
                    quantity,
                    product.Category.NameCate,
                    selectedSize,
                    selectedColor
                );
            }
            return RedirectToAction("Index");
        }

        // xóa sản phẩm khỏi giỏ
        public ActionResult RemoveFromCart(int id)
        {
            var cartService = GetCartService();
            cartService.GetCart().RemoveItem(id);
            return RedirectToAction("Index");
        }

        // Làm trống giỏ hàng
        public ActionResult ClearCart()
        {
            GetCartService().ClearCart();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult UpdateQuantity(int id, int quantity)
        {
            var cartService = GetCartService();
            cartService.GetCart().UpdateQuantity(id, quantity);
            return RedirectToAction("Index");
        }
        public ActionResult DecreaseQuantity(int id)
        {
            var cartService = GetCartService();
            var cart = cartService.GetCart();
            var item = cart.Items.FirstOrDefault(i => i.idPro == id);
            if (item != null)
            {
                if (item.quantity > 1)
                {
                    item.quantity--;
                }
                else
                {
                    cart.RemoveItem(id);
                }
            }
            return RedirectToAction("Index");
        }

        public ActionResult IncreaseQuantity(int id)
        {
            var cartService = GetCartService();
            var cart = cartService.GetCart();
            var item = cart.Items.FirstOrDefault(i => i.idPro == id);
            if (item != null)
            {
                item.quantity++;
            }
            return RedirectToAction("Index");
        }
    }
}
