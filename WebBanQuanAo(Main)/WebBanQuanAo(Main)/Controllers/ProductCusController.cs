using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanQuanAo_Main_.Models;

namespace WebBanQuanAo_Main_.Controllers
{
    public class ProductCusController : Controller
    {
        DBClothingStoreEntities db = new DBClothingStoreEntities();
        // GET: ProductCus
        public ActionResult Index(int? cateId)
        {
            var products = db.Products.AsQueryable();

            if (cateId != null)
            {
                products = products.Where(p => p.IDCate == cateId);
            }

            ViewBag.Categories = db.Categories.ToList();
            ViewBag.Suppliers = db.Suppliers.ToList();
            ViewBag.Colors = db.Colors.ToList();
            ViewBag.Sizes = db.Sizes.ToList();

            return View(products.ToList());
        }
        public ActionResult ChiTiet(int id)
        {
            var product = db.Products.Find(id);
            if (product == null)
                return HttpNotFound();
            // Lấy tất cả biến (size, màu, giá)
            var products = db.Products.Where(x => x.ProductID == id).ToList();
            ViewBag.Sizes = products.Select(x => x.Size).Distinct().ToList();
            ViewBag.Colors = products.Select(x => x.Color).Distinct().ToList();

            return View(product);
        }
    }
}