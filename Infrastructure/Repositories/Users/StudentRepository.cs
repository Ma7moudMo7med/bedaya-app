using Core.DTOs.TeacherDashboardDTOs.StudentsDTO;
using Core.Interfaces.Users;
using Core.Models.Courses;
using Core.Models.Enrollments;
using Core.Models.Subscriptions;
using Core.Models.Users;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Users
{
    public class StudentRepository : IStudentRepository
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;
        public StudentRepository(AppDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IdentityResult> CreateStudentAsync(Student student, string password)
        {
            var result = await _userManager.CreateAsync(student, password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(student, "Student");
            }

            return result;
        }
        public async Task<IdentityResult> DeleteStudentAsync(Student student)
        {
            return await _userManager.DeleteAsync(student);
        }

        public async Task<List<Student>> GetAll()
        {
            return _context.Students.ToList();
        }

        public async Task<Student> GetStudentByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            return user as Student;
        }

        public async Task<IdentityResult> UpdateStudentAsync(Student student)
        {
            return await _userManager.UpdateAsync(student);
        }
        public async Task<int> GetTotalStudentCountAsync()
        {
            return await _context.Students.CountAsync();
        }

        //public async Task<List<Course>> GetStudentCourses(Guid studentId)
        //{
        //    return await _context.Enrollments
        //        .Where(e => e.StudentId == studentId)
        //        .Select(e => e.Course)
        //        .ToListAsync();
        //}
        public async Task<List<Enrollment>> GetStudentEnrollments(Guid studentId)
        {
            return await _context.Enrollments
                .Include(e => e.Course)
                    .ThenInclude(c => c.Teacher)
                .Where(e => e.StudentId == studentId)
                .OrderByDescending(e => e.EnrolledAt)
                .ToListAsync();
        }

        public async Task<SubscriptionPlan?> GetStudentActiveSubscriptionAsync(Guid studentId)
        {
            return await _context.SubscriptionPlans
                .Where(s => s.UserId == studentId && s.IsActive == true)
                .OrderByDescending(s => s.StartDate)
                .FirstOrDefaultAsync();
        }



        // Mahmoud / For TeacherDashboard - Students
        public async Task<StudentDetailsDTO> GetStudentDetailsAsync(Guid teacherId, Guid studentId)
        {
            var student = await _context.Students
            .FirstOrDefaultAsync(s => s.Id == studentId);

            if (student == null)
                return null;

            var courses = await _context.Courses
                .Where(c => c.TeacherId == teacherId)
                .Select(c => new
                {
                    Course = c,
                    Enrollment = c.Enrollments
                        .Where(e => e.StudentId == studentId)
                        .FirstOrDefault()
                })
                .Where(x => x.Enrollment != null)
                .Select(x => new StudentCourseDTO
                {
                    CourseId = x.Course.Id,
                    CourseTitle = x.Course.Title,
                    CourseDescription = x.Course.Description,
                    CourseImage = x.Course.ThumbnailUrl,
                    EnrolledAt = x.Enrollment.EnrolledAt,
                    Status = x.Enrollment.Status
                })
                .ToListAsync();

            return new StudentDetailsDTO
            {
                StudentId = student.Id,
                FullName = student.FullName,
                Email = student.Email,
                PhoneNumber = student.PhoneNumber,
                ProfilePicture = student.ProfilePicture,
                CoursesCount = courses.Count,
                Courses = courses
            };
        }
    }
}
