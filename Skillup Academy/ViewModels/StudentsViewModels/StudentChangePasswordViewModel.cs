namespace Skillup_Academy.ViewModels.StudentsViewModels
{
    public class StudentChangePasswordViewModel
    {
        public required string CurrentPassword { get; set; }
        public required string NewPassword { get; set; }
        public required string ConfirmNewPassword { get; set; }
    }
}
