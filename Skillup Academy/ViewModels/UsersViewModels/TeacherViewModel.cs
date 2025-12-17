using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
namespace Skillup_Academy.ViewModels.UsersViewModels
{
    public class TeacherViewModel
    {
        [ValidateNever]
        public string Id { get; set; }

        [Required(ErrorMessage = "Password is required for new teachers")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        // ------------------------------------------

        [Required]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string FullName { get; set; }

        public string? PhoneNumber { get; set; }
        public bool IsActive { get; set; }

        [ValidateNever]
        public DateTime? RegistrationDate { get; set; }

        [ValidateNever]
        public DateTime? LastLoginDate { get; set; }

        [ValidateNever]
        public int? TotalCourses { get; set; }

        [ValidateNever]
        public IFormFile? ClientFile { get; set; }
    }
}