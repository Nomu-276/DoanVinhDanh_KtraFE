using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanQuanAo_Main_.Models;

namespace WebBanQuanAo_Main_.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        DBClothingStoreEntities1 db =new DBClothingStoreEntities1();
        // GET: Admin/Home
        public ActionResult IndexCus()
        {
            var products = db.Products
                    .OrderByDescending(d => d.CreatedDate)
                    .Take(12)
                    .ToList();

            return View(products);
        }
        public ActionResult Index()
        {
            return View();
        }
    }
}