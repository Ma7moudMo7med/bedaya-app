using Core.DTOs.TeacherDashboardDTOs;
using Core.DTOs.TeacherDashboardDTOs.StudentsDTO;
using Core.Enums;
using Core.Interfaces.Users;
using Core.Models.Users;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Users
{
    public class TeacherRepository : ITeacherRepository
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IStudentRepository _studentRepository;

        public TeacherRepository(AppDbContext context, UserManager<User> userManager, IStudentRepository studentRepository)
        {
            _context = context;
            _userManager = userManager;
            _studentRepository = studentRepository;
        }

        public async Task<Teacher?> GetTeacherAsync(string? teacherId)
        {
            if (string.IsNullOrEmpty(teacherId))
                return null;
            return await _context.Teachers
                .FirstOrDefaultAsync(t => t.Id.ToString() == teacherId);
        }

        public async Task<List<CourseDashboardDTO>> GetTeacherCoursesAsync(Guid teacherId)
        {
            var Courses = await _context.Courses
                .Where(c => c.TeacherId == teacherId)
                .OrderByDescending(c => c.CreatedDate)
                .Include(c => c.Lessons)
                .Include(c => c.Enrollments)
                .Select(c => new CourseDashboardDTO
                {
                    CourseId = c.Id,
                    Title = c.Title,
                    Description = c.Description,
                    IsPublished = c.IsPublished,
                    IsFree = c.IsFree,
                    ThumbnailUrl = c.ThumbnailUrl,
                    AverageRating = c.AverageRating,
                    TotalDuration = c.TotalDuration,
                    CreatedDate = c.CreatedDate,
                    TotalLessons = c.Lessons.Count,
                    TotalStudents = c.Enrollments.Count,
                    teacherId = c.TeacherId

                })
                .ToListAsync();
            return Courses;
        }

        public async Task<int> GetTotalStudentsAsync(Guid teacherId)
        {
            return await _context.Courses
                    .Where(c => c.TeacherId == teacherId)
                    .SelectMany(c => c.Enrollments.Select(e => e.StudentId))
                    .Distinct()
                    .CountAsync();
        }

        public async Task<TeacherDashboardDTO> GetTeacherDashboardAsync(Guid teacherId)
        {
            // var teacher = await _context.Teachers.FindAsync(teacherId);
            var teacher = await GetTeacherAsync(teacherId.ToString());
            if (teacher == null) 
                return null;

            var courses = await GetTeacherCoursesAsync(teacherId);
            var totalStudents = await GetTotalStudentsAsync(teacherId);


            return new TeacherDashboardDTO
            {
                TeacherName = teacher.FullName,
                TotalCourses = courses.Count,
                TotalStudents = totalStudents,
                ProfilePictureUrl = teacher.ProfilePicture,
                TeacherBio = teacher.Bio,
                TeacherExpertise = teacher.Expertise,
                TeacherRating = teacher.Rating,
                Courses = courses
            };
        }


        public async Task<TeacherInfoDTO> GetTeacherInfoAsync(Guid teacherId)
        {
            var teacher = await GetTeacherAsync(teacherId.ToString());

            if (teacher == null)
                return null;

            return new TeacherInfoDTO
            {
                FullName = teacher.FullName,
                Email = teacher.Email,
                PhoneNumber = teacher.PhoneNumber,
                ProfilePicture = teacher.ProfilePicture,
                Bio = teacher.Bio,
                Qualifications = teacher.Qualifications,
                Expertise = teacher.Expertise
            };
        }

        public async Task<IdentityResult> UpdateTeacherInfoAsync(TeacherInfoDTO teacher, Guid teacherId)
        {
            var user = await GetTeacherAsync(teacherId.ToString());
            if (user == null)
                return (IdentityResult.Failed(new IdentityError { Description = "Teacher not found." }));
            // لليوزر  تحديث البيانات
            user.FullName = teacher.FullName;
            user.Email = teacher.Email;
            user.PhoneNumber = teacher.PhoneNumber;
            user.ProfilePicture = teacher.ProfilePicture;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
                return (IdentityResult.Failed(new IdentityError { Description = "Update failed." }));

            // الاضافيه تحديث البيانات
            //var teacherToUpdate = await _context.Users
            //                            .OfType<Teacher>()
            //                            .FirstOrDefaultAsync(t => t.Id == teacherId);
            var teacherToUpdate = await _context.Teachers
                                        .FirstOrDefaultAsync(t => t.Id == teacherId);
            if (teacherToUpdate == null)
                return (IdentityResult.Failed(new IdentityError { Description = "Teacher not found in context." }));
            teacherToUpdate.Bio = teacher.Bio;
            teacherToUpdate.Qualifications = teacher.Qualifications;
            teacherToUpdate.Expertise = teacher.Expertise;
            _context.Teachers.Update(teacherToUpdate);
            await _context.SaveChangesAsync();

            return (IdentityResult.Success);
        }

        public async Task<StudentDetailsDTO> GetStudentDetailsAsync(Guid teacherId, Guid studentId)
        {
            return await _studentRepository.GetStudentDetailsAsync(teacherId, studentId);
            //    //var student = await _userManager.Users.OfType<Student>()
            //    //    .FirstOrDefaultAsync(s => s.Id == studentId);
            //    var student = await _context.Students
            //        .FirstOrDefaultAsync(s => s.Id == studentId);
            //    if (student == null)
            //        return null;

            //    var courses = await _context.Courses
            //.Where(c => c.TeacherId == teacherId)
            //.Where(c => c.Enrollments.Any(e => e.StudentId == studentId))
            //.Select(c => new StudentCourseDTO
            //{
            //    CourseId = c.Id,
            //    CourseTitle = c.Title
            //})
            //.ToListAsync();

            //    return new StudentDetailsDTO
            //    {
            //        StudentId = student.Id,
            //        FullName = student.FullName,
            //        Email = student.Email,
            //        PhoneNumber = student.PhoneNumber,
            //        ProfilePicture = student.ProfilePicture,
            //        CoursesCount = courses.Count,
            //        Courses = courses
            //    };
        }

         public Task<int> TotalStudentOfCourse(Guid CourseId, Guid teacherId)
        {
            var count = _context.Enrollments
                .Where(e => e.CourseId == CourseId && e.Course.TeacherId == teacherId)
                .Select(e => e.StudentId)
                .Distinct()
                .Count();
            return Task.FromResult(count);
        }

        public async Task<StudentListDTO> GetStudentsAsync(Guid teacherId, int pageNumber, int pageSize, string? searchQuery, Guid? courseId, string? status)
        {
            // 1. نجيب الطلاب المرتبطين بالكورسات بتاعة المدرس
            //var studentsQuery = _context.Users
            //    .OfType<Student>() // Student كلاس بيرث من IdentityUser
            //    .Where(s => s.Enrollments
            //    .Any(sc => sc.Course.TeacherId == teacherId))
            //    .AsQueryable();

            var studentsQuery = _context.Students
                    .Where(s => s.Enrollments
                    .Any(e => e.Course.TeacherId == teacherId))
                    .AsQueryable();

            // 2. تطبيق فلتر البحث بالكلمة المفتاحية
            if (!string.IsNullOrEmpty(searchQuery))
            {
                studentsQuery = studentsQuery.Where(s =>
                    s.FullName.Contains(searchQuery) ||
                    s.Email.Contains(searchQuery));
            }
            // 3. فلتر بالكورس
            if (courseId.HasValue)
            {
                studentsQuery = studentsQuery.Where(s =>
                    s.Enrollments.Any(sc => sc.CourseId == courseId.Value));
            }
            // 4. فلتر بالحالة
            StudentStatus? selectedStatusEnum = null;
            if (!string.IsNullOrEmpty(status) && 
                Enum.TryParse<StudentStatus>(status, out var statusEnum))
            {
                studentsQuery = studentsQuery.Where(s => s.StudentStatus == statusEnum);
                selectedStatusEnum = statusEnum; // هنا بنخزن القيمة
            }

            // 5. إجمالي الطلاب قبل Paging
            var totalRecords = await studentsQuery.CountAsync();

            // 6. Paging وجلب البيانات
            
            var students = await studentsQuery
       .OrderBy(s => s.FullName)
       .Skip((pageNumber - 1) * pageSize)
       .Take(pageSize)
       .Select(s => new StudentDTO
       {
           StudentId = s.Id,
           FullName = s.FullName,
           Email = s.Email,
           CoursesCount = s.Enrollments.Count(),
           CourseTitle = s.Enrollments
               .Select(e => e.Course.Title)
               .FirstOrDefault(),
           Status = s.StudentStatus
       })
       .ToListAsync();
            // 7. احصائيات الـDashboard
            //var totalStudents = await _context.Users.OfType<Student>()
            //    .Where(s => s.Enrollments.Any(sc => sc.Course.TeacherId == teacherId))
            //    .CountAsync();

            //var activeStudents = await _context.Users.OfType<Student>()
            //    .Where(s => s.Enrollments.Any(sc => sc.Course.TeacherId == teacherId) && s.StudentStatus == StudentStatus.Active)
            //    .CountAsync();

            //var inactiveStudents = await _context.Users.OfType<Student>()
            //    .Where(s => s.Enrollments.Any(sc => sc.Course.TeacherId == teacherId) && s.StudentStatus == StudentStatus.Inactive)
            //    .CountAsync();
            var stats = await _context.Students
             .Where(s => s.Enrollments
             .Any(e => e.Course.TeacherId == teacherId))
             .GroupBy(s => 1)
             .Select(g => new
             {
                 Total = g.Count(),
                 Active = g.Count(s => s.StudentStatus == StudentStatus.Active),
                 Inactive = g.Count(s => s.StudentStatus == StudentStatus.Inactive)
             })
             .FirstOrDefaultAsync();
            // كورسات المدرس
       //     var teacherCourses = await _context.Courses
       //.Where(c => c.TeacherId == teacherId)
       //.Select(c => new CourseDashboardDTO
       //{
       //    CourseId = c.Id,
       //    Title = c.Title
       //})
       //.ToListAsync();
              var teacherCourses = await GetTeacherCoursesAsync(teacherId);
            return new StudentListDTO
            {
                Students = students,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                SearchQuery = searchQuery,
                SelectedCourseId =courseId,/*courseId.HasValue ? (Guid?)courseId.Value : null*/
                SelectedStatus = selectedStatusEnum,
                TotalStudents = stats?.Total ?? 0,
                ActiveStudents = stats?.Active ?? 0,
                InactiveStudents = stats?.Inactive ?? 0,
                TeacherCourses = teacherCourses
            };

        }
        public Task<int> GetActiveStudentsAsync(Guid teacherId)
        {
            var count = _context.Users
                .OfType<Student>()
                .Where(s => s.Enrollments.Any(e => e.Course.TeacherId == teacherId) && s.StudentStatus == StudentStatus.Active)
                .Count();
            return Task.FromResult(count);
        }

        public Task<int> GetCompleteStudentsAsync(Guid teacherId)
        {
            var count = _context.Users
                .OfType<Student>()
                .Where(s => s.Enrollments.Any(e => e.Course.TeacherId == teacherId) && s.StudentStatus == StudentStatus.Completed)
                .Count();
            return Task.FromResult(count);
        }


        // for Admin Dashboard
        public async Task<IdentityResult> CreateTeacherAsync(Teacher teacher, string password)
        {
            var result = await _userManager.CreateAsync(teacher, password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(teacher, "Instructor");
            }

            return result;
        }


        public async Task<IdentityResult> DeleteTeacherAsync(Teacher teacher)
        {
            return await _userManager.DeleteAsync(teacher);
        }

        public async Task<List<Teacher>> GetAll()
        {
            return await _context.Set<Teacher>().ToListAsync();
        }

        public async Task<IdentityResult> UpdateTeacherAsync(Teacher teacher)
        {
            return await _userManager.UpdateAsync(teacher);
        }
        public async Task<int> GetTotalTeacherCountAsync()
        {
            return await _context.Teachers.CountAsync();
        }

        
    }
}
