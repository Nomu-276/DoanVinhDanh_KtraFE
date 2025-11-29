using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace WebBanQuanAo_Main_.Models.ViewModel
{
    public class Cart
    {
        private List<CartItem> items = new List<CartItem>();

        public IEnumerable<CartItem> Items => items;

        //danh sach cac san phẩm trong gio hang da duoc nhom theo Category
        public List<IGrouping<string, CartItem>> GroupedItems => items.GroupBy(i => i.Category).ToList();

        // Các thuoc tinh hỗ trợ phan trang
        public int PageNumber { get; set; } // Trang hiện tại
        public int PageSize { get; set; } = 6; // Số sản phẩm mỗi trang

        //danh sách các sản phẩm cùng danh mục với các sản phẩm trong giỏ
        public PagedList.IPagedList<Product> SimilarProducts { get; set; }

        // Thêm sản phẩn vào giỏ
        public void AddItem(int productId, string productImage, string productName,
        decimal unitPrice, int quantity, string category, string size, string color)
        {
            var existingItem = items.FirstOrDefault(i => i.idPro == productId);
            if (existingItem == null)
            {
                items.Add(new CartItem
                {
                    idPro = productId,
                    image = productImage,
                    namePro = productName,
                    Price = unitPrice,
                    quantity = quantity,
                    Size = size,
                    Color = color
                });
            }
            else
            {
                existingItem.quantity += quantity;
            }
        }

        // Xóa sản phẩm khỏi giỏ
        public void RemoveItem(int productId)
        {
            items.RemoveAll(i => i.idPro == productId);
        }

        // Tính tổng giá trị giỏ hàng
        public decimal TotalValue()
        {
            return items.Sum(i => i.total);
        }

        // Lảm trống giỏ hàng
        public void Clear()
        {
            items.Clear();
        }

        // Cập nhật số lượng của sản phẩm đã chọn
        public void UpdateQuantity(int productId, int quantity)
        {
            var item = items.FirstOrDefault(i => i.idPro == productId);
            if (item != null)
            {
                item.quantity = quantity;
            }
        }
        public int TotalQuantity()
        {
            if (items == null) return 0;
            return items.Sum(s => s.quantity);
        }
    }
}