using Core.DTOs.TeacherDashboardDTOs.StudentsDTO;
using Core.Models.Courses;
using Core.Models.Enrollments;
using Core.Models.Subscriptions;
using Core.Models.Users;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Users
{
    public interface IStudentRepository
    {
        // CRUD Operations
        Task<List<Student>> GetAll();
        Task<Student> GetStudentByIdAsync(string id);

        Task<IdentityResult> UpdateStudentAsync(Student student);

      
        Task<IdentityResult> DeleteStudentAsync(Student student);
        Task<IdentityResult> CreateStudentAsync(Student student, string password);
        Task<int> GetTotalStudentCountAsync();

        // Additional Methods Specific to Students can be added here
        Task<StudentDetailsDTO> GetStudentDetailsAsync(Guid teacherId, Guid studentId);

        //Task<List<Course>> GetStudentCourses(Guid studentId);

        Task<SubscriptionPlan?> GetStudentActiveSubscriptionAsync(Guid studentId);

        Task<List<Enrollment>> GetStudentEnrollments(Guid studentId);

    }
}
