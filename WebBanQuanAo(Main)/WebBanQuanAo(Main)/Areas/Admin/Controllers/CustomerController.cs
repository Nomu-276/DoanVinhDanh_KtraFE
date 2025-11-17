using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanQuanAo_Main_.Models;

namespace WebBanQuanAo_Main_.Areas.Admin.Controllers
{
    
    public class CustomerController : Controller
        
    {
        DBClothingStoreEntities db = new DBClothingStoreEntities();
        // GET: Admin/Customer
        public ActionResult QuanLiKhachHang()
        {
            return View(db.Customers.ToList());
        }
        public ActionResult Them()
        {
            return View();
        }
    }
}