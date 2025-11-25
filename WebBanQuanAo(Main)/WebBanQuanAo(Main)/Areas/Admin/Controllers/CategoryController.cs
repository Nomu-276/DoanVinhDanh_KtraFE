using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanQuanAo_Main_.Models;

namespace WebBanQuanAo_Main_.Areas.Admin.Controllers
{
    public class CategoryController : Controller
    {
        // GET: Admin/Category
        //khởi tạo database
        DBClothingStoreEntities1 db = new DBClothingStoreEntities1();

        // GET: Category
        public ActionResult DanhSachDanhMuc()
        {
            var cate = db.Categories.ToList();
            return View(cate);                  
        }


        [HttpGet]
        public ActionResult Them()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Them(Category cate)
        {
            db.Categories.Add(cate);
            db.SaveChanges();
            return RedirectToAction("DanhSachDanhMuc");
        }
        [HttpGet]
        public ActionResult Sua(int id)
        {
            var cate = db.Categories.Find(id);
            return View(cate);
        }
        [HttpPost]
        public ActionResult Sua(int id, Category cate)
        {
            var cate2 = db.Categories.Find(id);
            cate2.NameCate = cate.NameCate;
            db.SaveChanges();
            return RedirectToAction("DanhSachDanhMuc");
        }
        [HttpGet]
        public ActionResult Xoa(int id)
        {
            var cate = db.Categories.Find(id);
            if (cate == null)
                return HttpNotFound();
            return View(cate);
        }
        [HttpPost]
        public ActionResult Xoa(int id, Category cate)
        {
            cate = db.Categories.Find(id);
            if (cate == null)
                return HttpNotFound();

            bool hasProducts = db.Products.Any(p => p.IDCate == id);

            if (hasProducts)
            {
            
                ViewBag.Error = "Không thể xóa vì danh mục đang chứa sản phẩm!";
                return View(cate);
            }

            db.Categories.Remove(cate);
            db.SaveChanges();
            return RedirectToAction("DanhSachDanhMuc");
        }
        public ActionResult Detail(int id)
        {
            var category = db.Categories
                .Include("Products")
                .FirstOrDefault(c => c.IDCate == id);
            if (category == null)
            {
                return HttpNotFound();
            }

            return View(category);
        }



    }
}