using Core.Models.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Skillup_Academy.Controllers.ViewComponents
{
    public class AdminProfileViewComponent : ViewComponent
    {
        private readonly UserManager<User> _userManager;

        public AdminProfileViewComponent(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (user == null)
            {
                return View(new
                {
                    FullName = "Guest Admin",
                    ProfilePicture = "~/img/undraw_profile.svg"
                });
            }

            var model = new
            {
                FullName = user.FullName,
                ProfilePicture = user.ProfilePicture ?? "~/img/undraw_profile.svg"
            };

            return View(model);
        }
    }
}
