using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebBanQuanAo_Main_.Models.ViewModel
{
    public class LoginVM
    {
        [Required(ErrorMessage = "Vui lòng nhập tên đăng nhập")]
        [StringLength(30, MinimumLength = 5, ErrorMessage = "Tên đăng nhập từ 5 đến 30 ký tự")]
        [RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "Tên đăng nhập chỉ được chứa chữ cái, số và dấu gạch dưới")]
        [Display(Name = "Tên đăng nhập")]
        public string NameUser { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]

        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string PasswordUser { get; set; }

    }
}