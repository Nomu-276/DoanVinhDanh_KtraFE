using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using WebBanQuanAo_Main_.Models;


namespace WebBanQuanAo_Main_.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        DBClothingStoreEntities1 db = new DBClothingStoreEntities1();

        // GET: Product
        public ActionResult IndexCustomer(int? cateId)
        {
            var products = db.Products.AsQueryable();

            if (cateId != null)
            {
                products = products.Where(p => p.IDCate == cateId);
            }

            ViewBag.Categories = db.Categories.ToList();
            ViewBag.Suppliers = db.Suppliers.ToList();
            ViewBag.Colors = db.Colors.ToList();
            ViewBag.Sizes = db.Sizes.ToList();
            
            return View(products.ToList());
        }


        public ActionResult Index(string searchTerm)
        {
            var model = new ProductSearchVM();

            var products = db.Products.Include("Category").ToList()
                .AsQueryable();
            

            return View(products);

        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.listSup = new SelectList(db.Suppliers, "IDSup", "NameSup");
            ViewBag.listSize = new SelectList(db.Sizes, "IDSize", "SizeName");
            ViewBag.listColor = new SelectList(db.Colors, "IDColor", "ColorName");
            ViewBag.listCate = new SelectList(db.Categories, "IDCate", "NameCate");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product pro)
        {
            ViewBag.listSup = new SelectList(db.Suppliers, "IDSup", "NameSup");
            ViewBag.listSize = new SelectList(db.Sizes, "IDSize", "SizeName");
            ViewBag.listColor = new SelectList(db.Colors, "IDColor", "ColorName");
            ViewBag.listCate = new SelectList(db.Categories, "IDCate", "NameCate");

            if (ModelState.IsValid)
            {
                if (pro.ImagePath != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(pro.ImagePath.FileName);
                    string extension = Path.GetExtension(pro.ImagePath.FileName);
                    fileName = fileName + extension;
                    pro.ImagePath.SaveAs(Path.Combine(Server.MapPath("~/hinh/"), fileName));
                    pro.ImagePro = "~/hinh/" + fileName;
                }
                pro.CreatedDate = DateTime.Now;
                pro.SoldQuantity = 0;
                pro.ViewQuantity = 0;
                foreach (var item in ModelState)
                {
                    foreach (var error in item.Value.Errors)
                    {
                        System.Diagnostics.Debug.WriteLine(item.Key + ": " + error.ErrorMessage);
                    }
                }
                db.Products.Add(pro);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            foreach (var item in ModelState)
            {
                foreach (var error in item.Value.Errors)
                {
                    System.Diagnostics.Debug.WriteLine(item.Key + ": " + error.ErrorMessage);
                }
            }
            return View(pro);
        }


        [HttpGet]
        public ActionResult Sua(int id)
        {
            ViewBag.listSup = new SelectList(db.Suppliers, "IDSup", "NameSup");
            ViewBag.listSize = new SelectList(db.Sizes, "IDSize", "SizeName");
            ViewBag.listColor = new SelectList(db.Colors, "IDColor", "ColorName");
            ViewBag.listCate = new SelectList(db.Categories, "IDCate", "NameCate");
            var pro = db.Products.Find(id);
            return View(pro);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Sua(Product pro, int id)
        {
            ViewBag.listSup = new SelectList(db.Suppliers, "IDSup", "NameSup");
            ViewBag.listSize = new SelectList(db.Sizes, "IDSize", "SizeName");
            ViewBag.listColor = new SelectList(db.Colors, "IDColor", "ColorName");
            ViewBag.listCate = new SelectList(db.Categories, "IDCate", "NameCate");
            var proEdit = db.Products.Find(id);
            if (ModelState.IsValid)
            {
                
                proEdit.NamePro = pro.NamePro;
                proEdit.DescriptionPro = pro.DescriptionPro;
                proEdit.Size = pro.Size;
                proEdit.Color = pro.Color;
                proEdit.IDSup = pro.IDSup;
                proEdit.RemainQuantity = pro.RemainQuantity;
                proEdit.Price = pro.Price;
                proEdit.SoldQuantity = pro.SoldQuantity;
                proEdit.ViewQuantity = pro.ViewQuantity;
                
                proEdit.IDCate = pro.IDCate;
                if (pro.ImagePath != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(pro.ImagePath.FileName);
                    string extension = Path.GetExtension(pro.ImagePath.FileName);
                    fileName = fileName + extension;
                    pro.ImagePath.SaveAs(Path.Combine(Server.MapPath("~/hinh/"), fileName));
                    proEdit.ImagePro = "~/hinh/" + fileName;
                }
                db.SaveChanges();
                return RedirectToAction("Index");


            }
            return View(pro);
        }
        [HttpGet]
        public ActionResult Xoa(int id)
        {
            ViewBag.listSup = new SelectList(db.Suppliers, "IDSup", "NameSup");
            ViewBag.listSize = new SelectList(db.Sizes, "IDSize", "SizeName");
            ViewBag.listColor = new SelectList(db.Colors, "IDColor", "ColorName");
            ViewBag.listCate = new SelectList(db.Categories, "IDCate", "NameCate");
            var pro = db.Products.Find(id);
            return View(pro);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Xoa(Product pro, int id)
        {

            try
            {
                
                var proDelete = db.Products.Find(id);
                if (proDelete != null)
                {
                    ViewBag.listSup = db.Suppliers.ToList();
                    ViewBag.listSize = db.Sizes.ToList();
                    ViewBag.listColor = db.Colors.ToList();
                    ViewBag.listCate = db.Categories.ToList();
                    db.Products.Remove(proDelete);
                    db.SaveChanges();
                    ViewBag.errDelete = null;
                    return RedirectToAction("Index");
                }

            }
            catch
            {
                ViewBag.errDelete = "Không thể xóa vì sản phẩm này đang được sử dụng";
                return View(pro);

            }
            return View(pro);

        }

        public ActionResult ChiTiet(int id)
        {
            var product = db.Products.Find(id);
            if (product == null)
                return HttpNotFound();
            // Lấy tất cả biến (size, màu, giá)
            var products = db.Products.Where(x => x.ProductID == id).ToList();
            ViewBag.Sizes = products.Select(x => x.Size).Distinct().ToList();
            ViewBag.Colors = products.Select(x => x.Color).Distinct().ToList();

            return View(product);
        }
        public ActionResult ChiTietAdmin(int id)
        {
            var product = db.Products.Find(id);
            if (product == null)
                return HttpNotFound();
            // Lấy tất cả biến (size, màu, giá)
            var products = db.Products.Where(x => x.ProductID == id).ToList();
            ViewBag.Sizes = products.Select(x => x.Size).Distinct().ToList();
            ViewBag.Colors = products.Select(x => x.Color).Distinct().ToList();

            return View(product);
        }
    }
}