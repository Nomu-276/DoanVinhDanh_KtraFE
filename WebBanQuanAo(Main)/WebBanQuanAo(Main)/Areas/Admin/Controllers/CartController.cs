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
        DBClothingStoreEntities1 db = new DBClothingStoreEntities1();

        public List<CartItem> GetCartItems()
        {
            var cart = Session["Cart"] as List<CartItem>;
            if (cart == null)
            {
                cart = new List<CartItem>();
                Session["Cart"] = cart;
            }
            return cart;
        }

        public ActionResult Index()
        {
            var cart = GetCartItems();
            ViewBag.Total = cart.Sum(s => s.total);
            ViewBag.Count = cart.Sum(s => s.quantity);
            return View(cart);
        }

        [HttpPost]
        public ActionResult AddToCart(int ProductID, string color, string size)
        {
            var product = db.Products.FirstOrDefault(p => p.ProductID == ProductID);
            if (product == null)
                return RedirectToAction("IndexCustomer", "Product");

            var cart = GetCartItems();
            var existingItem = cart.FirstOrDefault(c => c.idPro == ProductID && c.color == color && c.size == size);

            if (existingItem != null)
            {
                existingItem.quantity++;
                return RedirectToAction("Index");
            }

            cart.Add(new CartItem
            {
                idPro = product.ProductID,
                namePro = product.NamePro,
                image = product.ImagePro,
                price = product.Price ?? 0,
                quantity = 1,
                color = color,
                size = size
            });

            return RedirectToAction("Index");
        }

        public ActionResult IncreaseQuantity(int id)
        {
            var cart = GetCartItems();
            var item = cart.FirstOrDefault(c => c.idPro == id);
            if (item != null)
                item.quantity++;

            return RedirectToAction("Index");
        }

        public ActionResult DecreaseQuantity(int id)
        {
            var cart = GetCartItems();
            var item = cart.FirstOrDefault(c => c.idPro == id);
            if (item != null && item.quantity > 1)
                item.quantity--;

            return RedirectToAction("Index");
        }

        public ActionResult RemoveFromCart(int id)
        {
            var cart = GetCartItems();
            var item = cart.FirstOrDefault(c => c.idPro == id);
            if (item != null)
                cart.Remove(item);

            return RedirectToAction("Index");
        }

    }
}