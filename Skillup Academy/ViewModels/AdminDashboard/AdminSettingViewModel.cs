using System.ComponentModel.DataAnnotations;

namespace Skillup_Academy.ViewModels.AdminDashboard
{
    public class AdminSettingViewModel
    {
        [Required(ErrorMessage = "Required")]
        [Display(Name = "FullName")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Email Requird")]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        //[Required(ErrorMessage = "Password Requird")]
        //[DataType(DataType.Password)]
        //[Display(Name = "Password")]
        //public string Password { get; set; }
        //[Required(ErrorMessage = "Password Confirmed Requird")]
        //[DataType(DataType.Password)]
        //[Display(Name = "Password Confirmed")]
        //public string PasswordConfirmed { get; set; }

        [Required(ErrorMessage = "PhoneNumber Requird")]
        [Display(Name = "PhoneNumber")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Current Profile Picture Path")]
        public string? CurrentProfilePicturePath { get; set; }

        [Display(Name = "Profile Picture")]
        public IFormFile? ProfilePicture { get; set; }
        public DateTime? RegistrationDate { get; set; } // = DateTime.Now;
        //public DateTime? LastLoginDate { get; set; } = DateTime.Now;    
        public DateTime? LastProfileUpdate { get; set; }

        //public bool IsActive { get; set; } = true;        
        //public bool CanViewPaidCourses { get; set; } = false;
    }
}
