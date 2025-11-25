using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanQuanAo_Main_.Models;

namespace WebBanQuanAo_Main_.Areas.Admin.Controllers
{

    public class LoginRegisterController : Controller
    {
        DBClothingStoreEntities1 db = new DBClothingStoreEntities1();
        // GET: LoginRegister
        //Tạo form login
        public ActionResult Index()
        {
            return View();
        }
        //Xử lý login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(AdminUser adminUser)
        {
            var checkID = db.AdminUsers.
                Where(s => s.ID.Equals(adminUser.ID)).FirstOrDefault();
            var checkPass = db.AdminUsers.
                Where(s => s.PasswordUser.Equals(adminUser.PasswordUser)).FirstOrDefault();

            if (checkID == null)
            {
                ViewBag.ErrLoginID = "Sai ID";
                return View("Index");
            }
            if (checkPass == null)
            {
                ViewBag.ErrLoginPass = "Sai Mật Khẩu";
                return View("Index");
            }
            if (checkID != null && checkPass != null)
            {
                Session["ID"] = adminUser.ID;
                Session["RoleUser"] = adminUser.RoleUser;
                if (Session["RoleUser"].ToString() == "admin")
                    return RedirectToAction("Index", "Product");
                else
                    return RedirectToAction("IndexCustomer", "Product");
            }
            return View("Index");

        }
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "LoginRegister");
        }
        //tạo form register
        public ActionResult Register()
        {
            return View();
        }
        //xác thực đăng kí
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(AdminUser adminUser)
        {
            var checkID = db.AdminUsers.Where(s => s.ID == adminUser.ID).FirstOrDefault();
            if (checkID != null)
            {
                ViewBag.ErrorRegister = "Trùng ID";
                return View("Register");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    db.AdminUsers.Add(adminUser);
                    db.SaveChanges();
                    return RedirectToAction("Index", "LoginRegister");
                }


            }

            return View();
        }
    }
}