using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanQuanAo_Main_.Models;
using WebBanQuanAo_Main_.Models.ViewModel;

namespace WebBanQuanAo_Main_.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        DBClothingStoreEntities db =new DBClothingStoreEntities();

        public ActionResult ThongKe()
        {
            // 1. Thực hiện truy vấn LINQ để nhóm và tính toán thống kê
            var statsData = db.Products // Truy cập bảng Sản phẩm
                .Where(p => p.Price.HasValue) // Chỉ lấy các sản phẩm có giá
                .GroupBy(p => p.Category.NameCate) // Nhóm theo Tên danh mục
                .Select(g => new ThongKeVM // Tạo View Model thống kê
                {
                    CategoryName = g.Key,
                    Count = g.Count(),
                    MinPrice = g.Min(p => p.Price.Value), // Giá Thấp Nhất
                    MaxPrice = g.Max(p => p.Price.Value), // Giá Cao Nhất
                    AvgPrice = g.Average(p => p.Price.Value) // Giá Trung Bình
                })
                .ToList();

            // 2. Chuyển đổi dữ liệu cho Biểu đồ (Google Charts yêu cầu cấu trúc mảng)
            // Chuẩn bị cho View
            ViewBag.StatsData = statsData;

            // Chuyển đổi dữ liệu thống kê thành JSON cho Google Chart
            // Biểu đồ cần tên danh mục và số lượng (Count)
            var chartData = statsData.Select(s => new object[] { s.CategoryName, s.Count }).ToList();

            // Thêm tiêu đề cột
            chartData.Insert(0, new object[] { "Loại Hàng", "Số lượng" });

            // Chuyển thành chuỗi JSON và lưu vào ViewBag để truyền sang View
            ViewBag.ChartDataJson = Newtonsoft.Json.JsonConvert.SerializeObject(chartData);

            return View(statsData);
        }
    }
}
