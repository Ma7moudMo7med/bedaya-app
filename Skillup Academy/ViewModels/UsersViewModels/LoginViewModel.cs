using System.ComponentModel.DataAnnotations;

namespace Skillup_Academy.ViewModels.UsersViewModels
{
	public class LoginViewModel
	{
 		public string Email { get; set; }
 		public string Password { get; set; }
		public bool RememberMe { get; set; }
	}
}
