using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebBanQuanAo_Main_.Models.ViewModel
{
    public class EditAccountVM
    {
        [Display(Name = "Họ và tên")]
        public string NameCus { get; set; }

        [Display(Name = "Tên đăng nhập")]
        public string NameUser { get; set; } 

        [Display(Name = "Số điện thoại")]
        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        public string PhoneCus { get; set; }

        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string EmailCus { get; set; }

        

        [Display(Name = "Ngày sinh")]
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        [Display(Name = "Mật khẩu mới")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Mật khẩu phải từ 6 ký tự")]
        public string NewPassword { get; set; }

        [Display(Name = "Xác nhận mật khẩu")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Mật khẩu xác nhận không khớp")]
        public string ConfirmPassword { get; set; }
        
        [Display(Name = "Địa chỉ")]
        [StringLength(150, MinimumLength = 8, ErrorMessage = "Mật khẩu phải có ít nhất 8 đến 150 ký tự")]
        public string AddressCus { get; set; }
    }
}
