using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CompareAttribute = System.ComponentModel.DataAnnotations.CompareAttribute;

namespace WebBanQuanAo_Main_.Models.ViewModel
{
    public class RegisterVM
    {


        [Required(ErrorMessage = "Vui lòng nhập tên đăng nhập")]
        [StringLength(30, MinimumLength = 5, ErrorMessage = "Tên đăng nhập từ 5 đến 30 ký tự")]
        [RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "Tên đăng nhập chỉ được chứa chữ cái, số và dấu gạch dưới")]
        [Display(Name = "Tên đăng nhập")]
        public string NameUser {  get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [StringLength(30, MinimumLength = 8, ErrorMessage = "Mật khẩu phải có ít nhất 8 đến 30 ký tự")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[!@#$%^&*()_+}{"";:?/>.<,])(?=.*[a-z])(?=.*\d).{8,}$",
        ErrorMessage = "Mật khẩu phải có ít nhất 8 ký tự, bao gồm chữ cái in hoa, chữ cái thường, số, và ký tự đặc biệt.")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string PasswordUser { get; set; }
        [NotMapped]
        [DataType(DataType.Password)]
        [Display(Name = "Xác nhận mật khẩu")]
        [Compare("PasswordUser", ErrorMessage = "Mật khẩu và xác nhận mật khẩu không khớp.")]
        [Required(ErrorMessage = "Vui lòng nhập xác nhận mật khẩu")]
        [StringLength(30, MinimumLength = 8, ErrorMessage = "Mật khẩu phải có ít nhất 8 đến 30 ký tự")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[!@#$%^&*()_+}{"";:?/>.<,])(?=.*[a-z])(?=.*\d).{8,}$",
        ErrorMessage = "Mật khẩu phải có ít nhất 8 ký tự, bao gồm chữ cái in hoa, chữ cái thường, số, và ký tự đặc biệt.")]
        public string ConfirmPassword { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập Họ và Tên")]
        [Display(Name ="Họ và Tên")]
        [StringLength(12, ErrorMessage = "Số điện thoại không quá 12 ký tự")]
        public string NameCus { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập Số điện thoại")]
        [Display(Name ="Số điện thoại")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneCus { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập Email")]
        [StringLength(150, ErrorMessage = "email không quá 150 ký tự")]
        [Display(Name ="Email")]
        [DataType(DataType.EmailAddress)]
        public string EmailCus { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập Địa chỉ")]
        [Display(Name ="Địa chỉ")]
        [StringLength(150, MinimumLength = 8, ErrorMessage = "Mật khẩu phải có ít nhất 8 đến 150 ký tự")]
        public string AddressCus { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập Ngày sinh")]
        [Display(Name ="Ngày sinh")]
        [DataType(DataType.DateTime)]
        public Nullable<System.DateTime> DateOfBirth { get; set; }
        
    }
}