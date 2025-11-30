using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

using WebBanQuanAo_Main_.Models;
using WebBanQuanAo_Main_.Models.ViewModel;

namespace WebBanQuanAo_Main_.Controllers
{
    public class AccountController : Controller
    {
        private DBClothingStoreEntities db = new DBClothingStoreEntities();

        // GET: Account/Register
        public ActionResult Register()
        {
            return View();
        }

        // POST: Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterVM model)
        {
            if (ModelState.IsValid)
            {
                // 1. Kiểm tra tên đăng nhập đã tồn tại chưa
                var checkUser = db.AdminUsers.FirstOrDefault(s => s.NameUser == model.NameUser);
                if (checkUser != null)
                {
                    ModelState.AddModelError("NameUser", "Tên đăng nhập đã tồn tại.");
                    return View(model);
                }
                else
                {
                // 2. Tạo bản ghi User mới
                    var user = new AdminUser
                    {
                        NameUser = model.NameUser,
                        PasswordUser = model.PasswordUser,

                        RoleUser = "Customer" // Gán quyền là Khách hàng
                    };
                db.AdminUsers.Add(user);

                // 3. Tạo bản ghi Customer mới
                var customer = new Customer
                {
                    NameCus = model.NameCus,
                    EmailCus = model.EmailCus,
                    PhoneCus = model.PhoneCus,
                    DateOfBirth = model.DateOfBirth,
                    AddressCus = model.AddressCus,
                    NameUser = model.NameUser
                };
                db.Customers.Add(customer);

                    db.SaveChanges();
              
               
                }
                    
            }

            return View(model);
        }

        // GET: Account/Login
        public ActionResult Login()
        {
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginVM model)
        {
            if (ModelState.IsValid)
            {
                var user = db.AdminUsers.SingleOrDefault(u => u.NameUser == model.NameUser
                                                      && u.PasswordUser == model.PasswordUser
                                                      && u.RoleUser == "Customer");

                if (user != null)
                {
                    Session["NameUser"] = user.NameUser;
                    Session["RoleUser"] = user.RoleUser;

                    FormsAuthentication.SetAuthCookie(user.NameUser, false);

                   
                    return RedirectToAction("Index", "HomeCus");
                }
                else
                {
                    ModelState.AddModelError(",", "Tên đăng nhập hoặc mật khẩu không đúng.");
                }
            }
            return View(model);
        }

        // GET: Account/Logout
        public ActionResult Logout()
        {
            Session.Clear(); // Xóa session
            FormsAuthentication.SignOut(); // Xóa cookie xác thực
            return RedirectToAction("Login", "Account");
        }
        public ActionResult ProfileInfo()
        {
            if (Session["NameUser"] == null)
            {
                return RedirectToAction("Login");
            }

            string currentUserName = Session["NameUser"].ToString();

            // Tìm khách hàng theo NameUser (đã thêm cột ở bước trước)
            var customer = db.Customers.FirstOrDefault(c => c.NameUser == currentUserName);

            if (customer == null) return RedirectToAction("ProfileInfo", "Account");

            
            var model = new EditAccountVM
            {
                NameCus = customer.NameCus,
                NameUser = customer.NameUser,
                PhoneCus = customer.PhoneCus,
                EmailCus = customer.EmailCus,
                AddressCus = customer.AddressCus, 
                DateOfBirth = customer.DateOfBirth
            };

            return View(model);
        }


        // GET: Hiển thị form sửa
        public ActionResult Index()
        {
            if (Session["NameUser"] == null) return RedirectToAction("Login");

            string currentUserName = Session["NameUser"].ToString();
            var customer = db.Customers.FirstOrDefault(c => c.NameUser == currentUserName);

            if (customer == null) return RedirectToAction("Index", "HomeCus");

            var model = new EditAccountVM
            {
                NameCus = customer.NameCus,
                NameUser = customer.NameUser,
                PhoneCus = customer.PhoneCus,
                EmailCus = customer.EmailCus,
                AddressCus= customer.AddressCus,
                DateOfBirth = customer.DateOfBirth,
                
            };

            return View(model);
        }

        // POST: Xử lý lưu dữ liệu
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(EditAccountVM model)
        {
            if (Session["NameUser"] == null) return RedirectToAction("Login");

            if (ModelState.IsValid)
            {
                try
                {
                    string currentUserName = Session["NameUser"].ToString();

                    // 1. Cập nhật bảng Customer
                    var customer = db.Customers.FirstOrDefault(c => c.NameUser == currentUserName);
                    if (customer != null)
                    {
                        customer.PhoneCus = model.PhoneCus;
                        customer.EmailCus = model.EmailCus;
                        customer.DateOfBirth = model.DateOfBirth;
                        customer.AddressCus = model.AddressCus; 
                    }

                    // 2. Cập nhật bảng AdminUser
                    if (!string.IsNullOrEmpty(model.NewPassword))
                    {
                        var user = db.AdminUsers.FirstOrDefault(u => u.NameUser == currentUserName);
                        if (user != null)
                        {
                            user.PasswordUser = model.NewPassword;
                        }
                    }

                    db.SaveChanges();
                    TempData["SuccessMessage"] = "Cập nhật hồ sơ thành công!";

                    // Load lại trang để thấy thay đổi
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Lỗi cập nhật: " + ex.Message);
                }
            }

            return View(model);
        }
    }

}
