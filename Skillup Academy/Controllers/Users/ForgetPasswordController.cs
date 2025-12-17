using Core.Models.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Skillup_Academy.ViewModels.UsersViewModels;

namespace Skillup_Academy.Controllers.Users
{
    public class ForgetPasswordController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IEmailSender _emailSender;
        public ForgetPasswordController(UserManager<User> UserManager, IEmailSender emailSender)
        {
            _userManager = UserManager;
            _emailSender = emailSender;
        }
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            ForgetPasswordViewModel FPVM = new ForgetPasswordViewModel();
            return View(FPVM);
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgetPasswordViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var resetLink = Url.Action("ResetPassword", "ForgetPassword",
                    new { token, email = user.Email }, Request.Scheme);
                await _emailSender.SendEmailAsync(user.Email, "Reset your password", resetLink);
            }
            return RedirectToAction("ForgotPasswordConfirmation");
        }
        [HttpGet]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ResetPassword(string token, string email)
        {
            if (token == null || email == null)
            {
                return RedirectToAction("Error", "Home");
            }
            var model = new ResetPasswordViewModel
            {
                Token = token,
                Email = email
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                return RedirectToAction("ResetPasswordConfirmation");
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);

            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }
    }
}