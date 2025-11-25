using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebBanQuanAo_Main_.Models
{
    [MetadataType(typeof(AdminUserMetadata))]
    public partial class AdminUser
    {
        [NotMapped]

        [Compare("PasswordUser", ErrorMessage = "Mật khẩu xác nhận không khớp")]
        public string ConfirmPassword { get; set; }
    }
    [MetadataType(typeof(ProductMetadata))]
    public partial class Product
    {
        [NotMapped]
        public HttpPostedFileBase ImagePath { get; set; }
        
        [NotMapped]
        public List<Product> NeedImportProduct { get; set; }
    }
    
    public class PartialClasses
    {
    }
}