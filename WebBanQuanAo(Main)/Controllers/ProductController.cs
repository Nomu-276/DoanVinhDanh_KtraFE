using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanQuanAo_Main_.Models;


namespace WebBanQuanAo_Main_.Controllers
{
    public class ProductController : Controller
    {
        
        // GET: Product
        public ActionResult Index()
        {
            return View();
        }
    }
}