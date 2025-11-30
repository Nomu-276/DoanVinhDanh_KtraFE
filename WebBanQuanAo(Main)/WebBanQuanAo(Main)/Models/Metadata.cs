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
        [StringLength(50, ErrorMessage = "Tên danh mục không được vượt quá 50 ký tự")]
        public string NameCate { get; set; }
        public virtual ICollection<Product> Products { get; set; }

    }
    public class AdminUserMetadata
    {

        public int ID { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập tên đăng nhập")]
        [StringLength(30, MinimumLength = 5, ErrorMessage = "Tên đăng nhập từ 5 đến 30 ký tự")]
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
        [StringLength(100,MinimumLength =4, ErrorMessage = "Tên sản phẩm không được vượt quá 100 ký tự và có ít nhất 4 ký tự")]
        [Display(Name = "Tên sản phẩm")]
        public string NamePro { get; set; }
        [StringLength(250, ErrorMessage = "Mô tả sản phẩm không được vượt quá 250 ký tự")]
        [Display(Name = "Mô tả sản phẩm")]
        public string DescriptionPro { get; set; }
        [Display(Name = "Mã danh mục")]
        public string IDCate { get; set; }
        [Display(Name = "Đường dẫn ảnh sản phẩm")]
        //[RegularExpression(@"^(https?:\/\/.*\.(?:png|jpg|jpeg|gif|jfif|bmp|webp))$", ErrorMessage = "Vui lòng nhập đúng định dạng URL hình ảnh")]
        //[DefaultValue("~/Content/hinh/default_img.jfif")]
        public string ImagePro { get; set; }
        public Nullable<int> IDSup { get; set; }
        public Nullable<int> IDColor { get; set; }
        public Nullable<int> IDSize { get; set; }
        // ...
        public virtual Size Size { get; set; }

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
        
        public virtual Supplier Supplier { get; set; }

        
    }
    public class ColorMetadata
    {
        [HiddenInput]
        public int IDColor { get; set; }
        [Required(ErrorMessage ="Vui lòng nhập tên màu")]
        [StringLength(70, MinimumLength = 4, ErrorMessage = "Tên màu không được vượt quá 70 ký tự và có ít nhất 4 ký tự")]
        public string ColorName { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
    public class SizeMetadata
    {
        [HiddenInput]
        public int IDSize { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập size")]
        [StringLength(8, MinimumLength = 1, ErrorMessage = "Nhập tên size từ 1 tới 8 ký tự")]
        public string SizeName { get; set; }
    }
    public class SupplierMetadata
    {
        [HiddenInput]
        public int IDSup { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập tên nhà cung cấp")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Nhập tên nhà cung cấp từ 1 tới 100 ký tự")]
        public string NameSup { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập địa chỉ nhà cung cấp")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Nhập địa chỉ từ 1 tới 100 ký tự")]
        public string AddressSup { get; set; }
    }
    public class CustomerMetadata
    {
        public int IDCus { get; set; }
        public string NameCus { get; set; }
        public Nullable<System.DateTime> DateOfBirth { get; set; }
        public string PhoneCus { get; set; }
        public string EmailCus { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderPro> OrderProes { get; set; }
        public virtual AdminUser AdminUser { get; set; }
    }
}