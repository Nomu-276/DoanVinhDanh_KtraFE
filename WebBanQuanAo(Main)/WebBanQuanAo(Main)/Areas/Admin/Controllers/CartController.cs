using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanQuanAo_Main_.Models;

namespace WebBanQuanAo_Main_.Areas.Admin.Controllers
{
    public class CartController : Controller
    {
        DBClothingStoreEntities db = new DBClothingStoreEntities();
        //tạo session giỏ hàng bằng danh sách các dòng hàng
        public List<CartItem> GetCartItems()
        {
            //khai báo biến giỏ hàng
            var cart = Session["Cart"] as List<CartItem>;
            if (cart == null)
            {
                cart = new List<CartItem>();
                Session["Cart"] = cart;
            }
            return cart;
        }
        // GET: Giao diện giỏ hàng
        public ActionResult Index()
        {
            var cart = GetCartItems();
            ViewBag.Total = cart.Sum(s => s.total);
            ViewBag.Count = cart.Sum(s => s.quantity);
            return View(cart);
        }
        [HttpPost]
        public ActionResult AddToCart(int proID, string color, string size)

        {
            var cart = GetCartItems();
            var pro = db.Products.Find(proID);

            // lấy đúng ProDetail theo màu + size
            var detail = db.ProDetails
                .FirstOrDefault(x =>
                    x.ProductID == proID &&
                    x.Color.ColorName == color &&
                    x.Size.SizeName == size
                );

            if (detail == null)
            {
                TempData["Error"] = "Sản phẩm này không tồn tại biến thể màu hoặc size!";
                return RedirectToAction("ChiTiet", "Product", new { id = proID });
            }

            var item = cart.FirstOrDefault(x =>
                x.idPro == proID &&
                x.color == color &&
                x.size == size
            );

            if (item != null)
            {
                item.quantity++;
            }
            else
            {
                cart.Add(new CartItem
                {
                    idPro = proID,
                    namePro = pro.NamePro,
                    image = pro.ImagePro,
                    color = color,
                    size = size,
                    quantity = 1,
                    price = detail.Price ?? 0
                });
            }

            return RedirectToAction("Index");
        }


        //Viết hàm xóa dòng hàng trong giỏ hàng
        public ActionResult Xoa(int id)
        {
            var cart = GetCartItems();
            var item = cart.FirstOrDefault(s => s.idPro == id);
            if (item != null) cart.Remove(item);
            return RedirectToAction("Index");
        }
        //Viết chức năng thanh toán
        public ActionResult ThanhToan(int IDCus, string DCNhanHang)
        {
            var cart = GetCartItems();
            if (!cart.Any())
            {
                TempData["Error"] = "Lỗi sản phẩm";
                return RedirectToAction("Index");
            }
            //nếu sản phẩm hợp lệ thì tạo đơn hàng
            var order = new OrderPro
            {
                DateOrder = DateTime.Now,
                IDCus = IDCus,
                AddressDeliverry = DCNhanHang,
                TotalAmount = cart.Sum(s => s.total)
            };
            db.OrderProes.Add(order);
            db.SaveChanges();
            //lưu dòng chi tiết vào OrderDetail
            foreach (var item in cart)
            {
                var orderdetail = new OrderDetail
                {
                    IDOrder = order.ID,
                    ID = item.idPro,
                    Quantity = item.quantity,
                    UnitPrice = item.price,

                };
                db.OrderDetails.Add(orderdetail);
            }
            db.SaveChanges();
            //sau khi lưu dữ liệu giỏ hàng rỗng
            Session["Cart"] = null;
            TempData["Success"] = "Thanh toán thành công!";
            return RedirectToAction("IndexCustomer", "Product");

        }
    }
}