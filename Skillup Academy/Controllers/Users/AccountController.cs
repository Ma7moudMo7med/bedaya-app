using Core.Enums;
using Core.Interfaces;
using Core.Models.Users;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Skillup_Academy.AppSettingsImages;
using Skillup_Academy.Helper;
using Skillup_Academy.ViewModels.UsersViewModels;
using System.Security.Claims;

namespace Skillup_Academy.Controllers.Users
{
	public class AccountController : Controller
	{
		private readonly UserManager<User> _userManager;
		private readonly SignInManager<User> _signInManager;
		private readonly FileService _fileService;
		private readonly SaveImage _saveImage;
		private readonly IRepository<Teacher> _repoTeacher;

		public AccountController(UserManager<User> UserManager, SignInManager<User> signInManager
			, FileService fileService, SaveImage saveImage, IRepository<Teacher> repoTeacher)
		{
			_userManager = UserManager;
			_signInManager = signInManager;
			_fileService = fileService;
			_saveImage = saveImage;
			_repoTeacher = repoTeacher;
		}


		public IActionResult AccessDenied()
		{
			return View();
		}

		[HttpGet]
		public IActionResult Login()
		{
			return View();
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Login(LoginViewModel account)
		{
			if (ModelState.IsValid)
			{
				User result = await _userManager.FindByEmailAsync(account.Email);
				if (result != null)
				{
					bool found = await _userManager.CheckPasswordAsync(result, account.Password);
					if (found)
					{
						await _signInManager.SignInAsync(result, account.RememberMe);
						result.LastLoginDate = DateTime.Now;
						if (User.IsInRole("Admin"))
						{
							return RedirectToAction("DashBoard", "Admin");
						}
						if (User.IsInRole("Instructor"))
						{
							return RedirectToAction("Dashboard", "Teacher");
						} 
                        return RedirectToAction(nameof(Index), "Home");
					}
				}

				ModelState.AddModelError(string.Empty, "User and password invaled");
			}
			return View(account);
		}


		[Authorize]
		[HttpGet]
		public async Task<IActionResult> Logout()
		{
			await _signInManager.SignOutAsync();
			return RedirectToAction(nameof(Index), "Home");
		}



		[HttpGet]
		public IActionResult InstructorRegister()
		{
			return View();
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> InstructorRegister(InstructorRegisterViewModel AccountUser)
		{
			if (ModelState.IsValid)
			{
				var file = _fileService.GetDefaultAvatar();

				var user = new Teacher
				{
					Email = AccountUser.Email,
					UserName = AccountUser.FirstName,
					RegistrationDate = DateTime.Now,
					Bio = AccountUser.Bio,
					Expertise = AccountUser.Expertise,
					Qualifications = AccountUser.Qualifications ?? "Non",
					LastLoginDate = DateTime.Now,
					LastProfileUpdate = DateTime.Now,
					FullName = AccountUser.FirstName + " " + AccountUser.LastName,
					PhoneNumber = AccountUser.PhoneNumber,
					ProfilePicture = AccountUser.ClientFile != null
							  ? await _saveImage.SaveImgAsync(AccountUser.ClientFile)
							  : await _saveImage.SaveImgAsync(file)
				};


				var result = await _userManager.CreateAsync(user, AccountUser.Password);
				if (result.Succeeded)
				{
					var roleResult = await _userManager.AddToRoleAsync(user, UserType.Instructor.ToString());
					if (roleResult.Succeeded)
					{
						await _signInManager.SignInAsync(user, AccountUser.RememberMe);
						return RedirectToAction("Dashboard", "Teacher");
					}
					else
					{
						foreach (var error in roleResult.Errors)
						{
							ModelState.AddModelError(string.Empty, error.Description);
						}
					}
				}
				else
				{
					foreach (var error in result.Errors)
					{
						ModelState.AddModelError(string.Empty, error.Description);
					}
				}
			}

			return View(AccountUser);
		}




		[HttpGet]
		public IActionResult StudentRegister()
		{
			return View();
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> StudentRegister(StudentRegisterViewModel AccountUser)
		{
			if (ModelState.IsValid)
			{
				var file = _fileService.GetDefaultAvatar();

				var user = new Student
				{
					Email = AccountUser.Email,
					UserName = AccountUser.FirstName,
					RegistrationDate = DateTime.Now,
					LastLoginDate = DateTime.Now,
					LastProfileUpdate = DateTime.Now,
					FullName = AccountUser.FirstName + " " + AccountUser.LastName,
					PhoneNumber = AccountUser.PhoneNumber,
					ProfilePicture = AccountUser.ClientFile != null
							  ? await _saveImage.SaveImgAsync(AccountUser.ClientFile)
							  : await _saveImage.SaveImgAsync(file)
				};


				var result = await _userManager.CreateAsync(user, AccountUser.Password);
				if (result.Succeeded)
				{
					var roleResult = await _userManager.AddToRoleAsync(user, UserType.Student.ToString());
					if (roleResult.Succeeded)
					{
						await _signInManager.SignInAsync(user, AccountUser.RememberMe);
						return RedirectToAction(nameof(Index), "Home");
					}
					else
					{
						foreach (var error in roleResult.Errors)
						{
							ModelState.AddModelError(string.Empty, error.Description);
						}
					}
				}
				else
				{
					foreach (var error in result.Errors)
					{
						ModelState.AddModelError(string.Empty, error.Description);
					}
				}
			}

			return View(AccountUser);
		}


		[HttpGet]
		public async Task<IActionResult> GetAllInstructors()
		{
			var teachers = await _repoTeacher.GetAllAsync();

			return View(teachers);
		}

		[HttpGet]
		public async Task<IActionResult> GetCoursesForInstructors(Guid Id)
		{
			var Courses = await _repoTeacher.Query()
				.Include(c => c.Courses)
				.FirstOrDefaultAsync(i => i.Id == Id);

			return View(Courses);
		}



		public IActionResult GoogleLogin()
		{
			var redirectUrl = Url.Action("GoogleResponse", "Account");
			var properties = _signInManager.ConfigureExternalAuthenticationProperties(GoogleDefaults.AuthenticationScheme, redirectUrl);
			return Challenge(properties, GoogleDefaults.AuthenticationScheme);
		}

        [HttpPost]
        public async Task<IActionResult> GoogleResponse()
        {
            // 1) Token from Google Identity Services
            var token = Request.Form["credential"];

            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Login");

            // 2) Validate token via Google
            var payload = await GoogleJsonWebSignature.ValidateAsync(token);

            var email = payload.Email;

            // 3) Check if user exists
            var user = await _userManager.FindByEmailAsync(email);

            // 4) Create user if not exists
            if (user == null)
            {
                user = new User
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true,
                    FullName = payload.Name ?? "Google User",
					ProfilePicture = payload.Picture ?? "\\img\\Profile\\download.png"
                };
                await _userManager.CreateAsync(user);
            }

            // 5) Sign in user
            await _signInManager.SignInAsync(user, isPersistent: false);

            // 6) Redirect anywhere you want
            return RedirectToAction(nameof(Index), "Home");
        }



    }


}
