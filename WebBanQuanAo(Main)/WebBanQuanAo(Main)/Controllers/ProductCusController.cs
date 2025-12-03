using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanQuanAo_Main_.Models;
using WebBanQuanAo_Main_.Models.ViewModel;

namespace WebBanQuanAo_Main_.Controllers
{
    public class ProductCusController : Controller
    {
        DBClothingStoreEntities db = new DBClothingStoreEntities();
        // GET: ProductCus
        public ActionResult Index(int? cateId, string searchTerm, decimal? minPrice, decimal?
            maxPrice, string sortOrder, int? page)
        {
            var model = new ProductSearchVM();
            var products = db.Products.AsQueryable();
            //Tìm kiếm theo từ khóa
            if (!string.IsNullOrEmpty(searchTerm))
            {
                products = products.Where(p => p.NamePro.Contains(searchTerm) ||
                p.DescriptionPro.Contains(searchTerm) ||
                p.Category.NameCate.Contains(searchTerm)
                
                );
            }
            // làm menu category trên thanh header

            if (cateId != null)
            {
                products = products.Where(p => p.IDCate == cateId);
            }
            //Tìm kiếm theo giá tối thiểu
            if (minPrice.HasValue)
            {
                products = products.Where(p => p.Price >= minPrice.Value);
            }
            if (maxPrice.HasValue)
            {
                products = products.Where(p => p.Price <= maxPrice.Value);
            }
            //Sắp xếp dựa trên lựa chọn của người dùng
            switch (sortOrder)
            {
                case "name_asc":
                    products = products.OrderBy(p => p.NamePro);
                    break;
                case "name_desc":
                    products = products.OrderByDescending(p => p.NamePro);
                    break;
                case "price_asc":
                    products = products.OrderBy(p => p.Price);
                    break;
                case "price_desc":
                    products = products.OrderByDescending(p => p.Price);
                    break;
                default:
                    products = products.OrderBy(p => p.NamePro);
                    break;
            }
            model.SortOrder = sortOrder;

            // đoạn code liên quan tới phân trang nếu
            // lấy số trang hiện tại (mặc định là 1 nếu không có giá trị)
            int pageNumber = page ?? 1;
            int pageSize = 16; // số sản phẩm trên mỗi trang

            //đóng lệnh này sử dụng ToPagedList lấy danh sách đã phân trang

            //model.Products = products.ToList();
            model.Products = products.ToPagedList(pageNumber, pageSize);
            

            //ViewBag.Categories = db.Categories.ToList();
            ViewBag.Suppliers = db.Suppliers.ToList();
            ViewBag.Colors = db.Colors.ToList();
            ViewBag.Sizes = db.Sizes.ToList();

            return View(model);
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
    }
}