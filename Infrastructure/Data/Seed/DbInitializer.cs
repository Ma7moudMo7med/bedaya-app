using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Enums;
using Core.Models.Users;
using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Data.Seed
{
	public class DbInitializer
	{
		private readonly RoleManager<Role> _roleManager;
		private readonly UserManager<User> _userManager;

		public DbInitializer(RoleManager<Role> roleManager, UserManager<User> userManager)
		{
			_roleManager = roleManager;
			_userManager = userManager;
		}

		public async Task SeedAdminAsync()
		{
 			foreach (UserType role in Enum.GetValues(typeof(UserType)))
			{
				if (!await _roleManager.RoleExistsAsync(role.ToString()))
				{
					await _roleManager.CreateAsync(new Role { Name = role.ToString() });
				}
			}

 			var adminEmail = "admin@gmail.com";
 			// var teacherEmail = "teacher@gmail.com";
			var adminUser = await _userManager.FindByEmailAsync(adminEmail);
			// var teacherUser = await _userManager.FindByEmailAsync(teacherEmail);

			if (adminUser == null)
			{
				var admin = new User
				{
					UserName = "admin",
					Email = adminEmail,
					FullName = "Main Administrator",
					EmailConfirmed = true,
					IsActive = true,
					UserType = UserType.Admin, 
					ProfilePicture = "avatar.png"
				};

				var result = await _userManager.CreateAsync(admin, "Admin@123");

				if (result.Succeeded)
				{
					await _userManager.AddToRoleAsync(admin, UserType.Admin.ToString());
 				}
				else
				{
					throw new Exception("❌ Failed to create the default administrator account");
				}
			}
			//if (teacherUser == null)
			//{
			//	var teacher = new User
			//	{
			//		UserName = "ali",
			//		Email = teacherEmail,
			//		FullName = "Main Teacher",
			//		EmailConfirmed = true,
			//		IsActive = true,
			//		UserType = UserType.Instructor,
			//		ProfilePicture = "avatar.png"
			//	};

			//	var result = await _userManager.CreateAsync(teacher, "Teacher@123");

			//	if (result.Succeeded)
			//	{
			//		await _userManager.AddToRoleAsync(teacher, UserType.Instructor.ToString());
			//	}
			//	else
			//	{
			//		throw new Exception("❌ Failed to create the default administrator account");
			//	}
			//}
		}
	}

}
