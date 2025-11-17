using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Mvc;
using WebBanQuanAo_Main_.Models;

namespace WebBanQuanAo_Main_.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        DBClothingStoreEntities db = new DBClothingStoreEntities();

        // GET: Product
        public ActionResult IndexCustomer(string cateId)
        {
            var products = db.Products.AsQueryable();

            // Nếu người dùng đã bấm vào danh mục
            if (!string.IsNullOrEmpty(cateId))
            {
                products = products.Where(p => p.IDCate == cateId);
            }

            // Load tất cả Category để hiển thị menu
            ViewBag.Categories = db.Categories.ToList();

            return View(products.ToList());
        }


        public ActionResult Index(decimal? _min, decimal? _max)
        {
            var proFilter = db.Products.AsQueryable();
            //if (_min.HasValue)
            //    proFilter = proFilter.Where(s => s.Price >= _min.Value);
            //if (_max.HasValue)
            //    proFilter = proFilter.Where(s => s.Price <= _max.Value);
            return View(proFilter.ToList());
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.listCate = new SelectList(db.Categories, "IDCate", "NameCate");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product pro)
        {
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
                db.Products.Add(pro);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(pro);
        }

        [HttpGet]
        public ActionResult Sua(int id)
        {
            var details = db.ProDetails.Where(x => x.ProductID == id).ToList();

            ViewBag.Details = details;
            ViewBag.Sizes = details.Select(x => x.Size).Distinct().ToList();
            ViewBag.Colors = details.Select(x => x.Color).Distinct().ToList();
            ViewBag.listCate = new SelectList(db.Categories, "IDCate", "NameCate");
            var pro = db.Products.Find(id);
            return View(pro);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Sua(Product pro, int id)
        {
            var details = db.ProDetails.Where(x => x.ProductID == id).ToList();

            ViewBag.Details = details;
            ViewBag.Sizes = details.Select(x => x.Size).Distinct().ToList();
            ViewBag.Colors = details.Select(x => x.Color).Distinct().ToList();
            ViewBag.listCate = new SelectList(db.Categories, "IDCate", "NameCate");
            var proEdit = db.Products.Find(id);
            if (ModelState.IsValid)
            {
                proEdit.NamePro = pro.NamePro;

                proEdit.DecriptionPro = pro.DecriptionPro;
                //proEdit.Price = pro.Price;
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
            var details = db.ProDetails.Where(x => x.ProductID == id).ToList();

            ViewBag.Details = details;
            ViewBag.Sizes = details.Select(x => x.Size).Distinct().ToList();
            ViewBag.Colors = details.Select(x => x.Color).Distinct().ToList();
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
                var details = db.ProDetails.Where(x => x.ProductID == id).ToList();

                ViewBag.Details = details;
                ViewBag.Sizes = details.Select(x => x.Size).Distinct().ToList();
                ViewBag.Colors = details.Select(x => x.Color).Distinct().ToList();
                ViewBag.listCate = new SelectList(db.Categories, "IDCate", "NameCate");
                var proDelete = db.Products.Find(id);
                if (proDelete != null)
                {
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
            var details = db.ProDetails.Where(x => x.ProductID == id).ToList();

            ViewBag.Details = details;
            ViewBag.Sizes = details.Select(x => x.Size).Distinct().ToList();
            ViewBag.Colors = details.Select(x => x.Color).Distinct().ToList();

            return View(product);
        }
        public ActionResult ChiTietAdmin(int id)
        {
            var product = db.Products.Find(id);
            if (product == null)
                return HttpNotFound();
            // Lấy tất cả biến (size, màu, giá)
            var details = db.ProDetails.Where(x => x.ProductID == id).ToList();

            ViewBag.Details = details;
            ViewBag.Sizes = details.Select(x => x.Size).Distinct().ToList();
            ViewBag.Colors = details.Select(x => x.Color).Distinct().ToList();

            return View(product);
        }
    }
}