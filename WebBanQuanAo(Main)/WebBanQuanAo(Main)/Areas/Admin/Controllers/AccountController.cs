using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanQuanAo_Main_.Models;

namespace WebBanQuanAo_Main_.Areas.Admin.Controllers
{
    public class AccountController : Controller
    {
        DBClothingStoreEntities1 db = new DBClothingStoreEntities1();
        // GET: Admin/Account
        public ActionResult Index()
        {
            return View(db.AdminUsers.ToList());
        }
        [HttpGet]
        public ActionResult Them()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Them(AdminUser user)
        {
           /* if (ModelState.IsValid)
            {*/
                db.AdminUsers.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            /*}
            return View();*/
        }
        [HttpGet]
        public ActionResult Sua(int id)
        {
            var user = db.AdminUsers.Find(id);
            return View(user);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Sua(int id, AdminUser user)
        {
            var user2=db.AdminUsers.Find(id);
            user2.NameUser = user.NameUser;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Xoa(int id)
        {
            var user = db.AdminUsers.Find(id);
            return View(user);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Xoa(int id,AdminUser user)
        {
            user = db.AdminUsers.Find(id);
            db.AdminUsers.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        
    }
}