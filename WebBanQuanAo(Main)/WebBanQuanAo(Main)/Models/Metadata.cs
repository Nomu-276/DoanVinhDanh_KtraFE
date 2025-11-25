using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebBanQuanAo_Main_.Models
{
    public class CategoryMetadata
    {
        [HiddenInput]
        public int IDCate { get; set; }
        
        [Required(ErrorMessage = "Vui lòng nhập tên cate")]
        [StringLength(50, ErrorMessage = "Tên cate không được vượt quá 50 ký tự")]
        public string NameCate { get; set; }
        public virtual ICollection<Product> Products { get; set; }

    }
    public class AdminUserMetadata
    {

        public int ID { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập tên đăng nhập")]
        [StringLength(30,MinimumLength =5, ErrorMessage = "Tên đăng nhập từ 5 đến 30 ký tự")]
        [RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "Tên đăng nhập chỉ được chứa chữ cái, số và dấu gạch dưới")]
        public string NameUser { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập vai trò")]
        public string RoleUser { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [DataType(DataType.Password)]
        public string PasswordUser { get; set; }
    }
    public class ProductMetadata
    {
        [Display(Name = "Mã sản phẩm")]
        public int ProductID { get; set; }
        [StringLength(50, ErrorMessage = "Tên sản phẩm không được vượt quá 50 ký tự")]
        [Display(Name = "Tên sản phẩm")]
        public string NamePro { get; set; }
        [Display(Name = "Mô tả sản phẩm")]
        public string DescriptionPro { get; set; }
        [Display(Name = "Mã danh mục")]
        public string IDCate { get; set; }
        [Display(Name = "Đường dẫn ảnh sản phẩm")]
        //[DefaultValue("~/Content/hinh/default_img.jfif")]
        public string ImagePro { get; set; }
        public Nullable<int> IDSup { get; set; }
        public Nullable<int> IDColor { get; set; }
        public Nullable<int> IDSize { get; set; }
        
        [Display(Name = "Đơn giá")]
        [Required(ErrorMessage = "Vui lòng nhập đơn giá")]
        [Range(50000, 2000000, ErrorMessage = "Vui lòng nhập giá từ 50000 tới 2000000")]
        public Nullable<decimal> Price { get; set; }
       [Display(Name = "Số lượng còn lại")]
        public Nullable<int> RemainQuantity { get; set; }
        [Display(Name = "Số lượng đã bán")]
        public Nullable<int> SoldQuantity { get; set; }
        [Display(Name = "Số lượt xem")]
        public Nullable<int> ViewQuantity { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }

        public virtual Category Category { get; set; }
        public virtual Color Color { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual Size Size { get; set; }
        public virtual Supplier Supplier { get; set; }

        
    }
    public class OrderProMetadata
    {
        public int ID { get; set; }
        public Nullable<System.DateTime> DateOrder { get; set; }
        public Nullable<int> IDCus { get; set; }
        public string AddressDelivery { get; set; }
        public Nullable<decimal> TotalAmount { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentStatus { get; set; }
        public string OrderStatus { get; set; }
    }
}