using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanQuanAo_Main_.Models;


namespace WebBanQuanAo_Main_.Controllers
{
    public class HomeCusController : Controller
    {
        private DBClothingStoreEntities  db= new DBClothingStoreEntities();
        public ActionResult Index()
        {
            var products = db.Products
                    .OrderByDescending(d => d.CreatedDate)
                    .Take(12)
                    .ToList();

            return View(products);
        }

        
    }
}