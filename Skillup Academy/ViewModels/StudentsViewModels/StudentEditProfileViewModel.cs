namespace Skillup_Academy.ViewModels.StudentsViewModels
{
    public class StudentEditProfileViewModel
    {
        public Guid Id { get; set; }

        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public string? CurrentProfilePicture { get; set; }

        public IFormFile? NewProfilePicture { get; set; }
    }
}
