using System.ComponentModel.DataAnnotations;

namespace Skillup_Academy.ViewModels.UsersViewModels
{
    public class ForgetPasswordViewModel
    {
        [Required, EmailAddress]
        public string Email { get; set; }
    }
}
