using System.ComponentModel.DataAnnotations;

namespace Skillup_Academy.ViewModels.UsersViewModels
{
	public class StudentRegisterViewModel
	{
		[Required(ErrorMessage = "Required")]
		[Display(Name = "FirstName")]
		public string FirstName { get; set; }

		[Required(ErrorMessage = "Required")]
		[Display(Name = "LastName")]
		public string LastName { get; set; }

		[Required(ErrorMessage = "Email Requird")]
		[EmailAddress(ErrorMessage = "Invalid Email")]
		[Display(Name = "Email")]
		public string Email { get; set; }



		[Required(ErrorMessage = "Password Requird")]
		[DataType(DataType.Password)]
		[Display(Name = "Password")]
		public string Password { get; set; }

		[Required(ErrorMessage = "Password Confirmed Requird")]
		[DataType(DataType.Password)]
		[Display(Name = "Password Confirmed")]
		public string PasswordConfirmed { get; set; }

		[Required(ErrorMessage = "PhoneNumber Requird")]
		[Display(Name = "PhoneNumber")]
		public string PhoneNumber { get; set; }

		[Display(Name = "Profile Picture")]
		public IFormFile? ClientFile { get; set; }

		[Display(Name = "RememberMe")]
		public bool RememberMe { get; set; }
	}
}
