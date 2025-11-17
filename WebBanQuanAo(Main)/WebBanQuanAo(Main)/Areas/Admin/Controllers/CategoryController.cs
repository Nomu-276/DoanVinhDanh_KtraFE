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
        DBClothingStoreEntities db = new DBClothingStoreEntities();

        // GET: Category
        public ActionResult DanhSachDanhMuc()
        {
            return View(db.Categories.ToList());
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
        public ActionResult Sua(string id)
        {
            var cate = db.Categories.Find(id);
            return View(cate);
        }
        [HttpPost]
        public ActionResult Sua(string id, Category cate)
        {
            var cate2 = db.Categories.Find(id);
            cate2.NameCate = cate.NameCate;
            db.SaveChanges();
            return RedirectToAction("DanhSachDanhMuc");
        }
        [HttpGet]
        public ActionResult Xoa(string id)
        {
            var cate = db.Categories.Find(id);
            return View(cate);
        }
        public ActionResult Xoa(string id, Category cate)
        {
            cate = db.Categories.Find(id.TrimEnd());
            db.Categories.Remove(cate);
            db.SaveChanges();
            return RedirectToAction("DanhSachDanhMuc");
        }
        public ActionResult ChiTiet(string id)
        {
            var cate = db.Categories.Find(id);
            return View(cate);
        }
    }
}