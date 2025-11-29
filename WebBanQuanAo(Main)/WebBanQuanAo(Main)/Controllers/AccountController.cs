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
                // 1. Kiểm tra tên đăng nhập đã tồn tại chưa [cite: 155]
                var checkUser = db.AdminUsers.FirstOrDefault(s => s.NameUser == model.NameUser);
                if (checkUser != null)
                {
                    ModelState.AddModelError("NameUser", "Tên đăng nhập đã tồn tại.");
                    return View(model);
                }
                else
                {
// 2. Tạo bản ghi User mới [cite: 158]
                    var user = new AdminUser
                    {
                        NameUser = model.NameUser,
                        PasswordUser = model.PasswordUser, // Lưu ý: Nên mã hóa mật khẩu trong thực tế

                        RoleUser = "Customer" // Gán quyền là Khách hàng
                    };
                db.AdminUsers.Add(user);

                // 3. Tạo bản ghi Customer mới [cite: 159]
                var customer = new Customer
                {
                    NameCus = model.NameCus,
                    EmailCus = model.EmailCus,
                    PhoneCus = model.PhoneCus,
                    DateOfBirth = model.DateOfBirth,
                    NameUser = model.NameUser // Liên kết với bảng User qua Username
                };
                db.Customers.Add(customer);

                // 4. Lưu cả 2 vào CSDL [cite: 160]
                //db.SaveChanges();
                try
                {
                    db.SaveChanges();
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException ex)
                {
                    // Lấy danh sách lỗi cụ thể
                    var errorMessages = ex.EntityValidationErrors
                            .SelectMany(x => x.ValidationErrors)
                            .Select(x => x.ErrorMessage);

                    // Nối các lỗi lại thành 1 chuỗi để hiển thị
                    var fullErrorMessage = string.Join("; ", errorMessages);

                    // Gán lỗi vào exception message để xem ngay trên màn hình vàng
                    var exceptionMessage = string.Concat(ex.Message, " Lỗi chi tiết: ", fullErrorMessage);

                    // Ném lỗi mới có kèm chi tiết ra ngoài
                    throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
                }

                // 5. Chuyển hướng về trang đăng nhập hoặc trang chủ
                return RedirectToAction("Login", "Account");
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
                // 1. Kiểm tra tài khoản trong DB (UserRole phải là Customer) [cite: 176]
                var user = db.AdminUsers.SingleOrDefault(u => u.NameUser == model.NameUser
                                                      && u.PasswordUser == model.PasswordUser
                                                      && u.RoleUser == "Customer");

                if (user != null)
                {
                    // 2. Lưu trạng thái đăng nhập vào Session [cite: 177]
                    Session["NameUser"] = user.NameUser;
                    Session["RoleUser"] = user.RoleUser;

                    // 3. Lưu cookie xác thực (để duy trì đăng nhập) [cite: 178]
                    FormsAuthentication.SetAuthCookie(user.NameUser, false);

                    // 4. Chuyển hướng về trang chủ [cite: 182]
                    return RedirectToAction("Index", "HomeCus");
                }
                else
                {
                    ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng.");
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

            // Map dữ liệu sang ViewModel
            var model = new EditAccountVM
            {
                NameCus = customer.NameCus,
                NameUser = customer.NameUser,
                PhoneCus = customer.PhoneCus,
                EmailCus = customer.EmailCus,
                //Address = customer.Address, // Bỏ comment nếu DB đã có cột Address
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
                
                DateOfBirth = customer.DateOfBirth
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

                    // 1. Cập nhật bảng Customer (Thông tin cá nhân)
                    var customer = db.Customers.FirstOrDefault(c => c.NameUser == currentUserName);
                    if (customer != null)
                    {
                        customer.PhoneCus = model.PhoneCus;
                        customer.EmailCus = model.EmailCus;
                        customer.DateOfBirth = model.DateOfBirth;
                        //customer.Address = model.Address; // Bỏ comment nếu DB đã có
                    }

                    // 2. Cập nhật bảng AdminUser (Nếu có nhập mật khẩu mới)
                    if (!string.IsNullOrEmpty(model.NewPassword))
                    {
                        var user = db.AdminUsers.FirstOrDefault(u => u.NameUser == currentUserName);
                        if (user != null)
                        {
                            user.PasswordUser = model.NewPassword; // Lưu ý: Nên mã hóa pass
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
