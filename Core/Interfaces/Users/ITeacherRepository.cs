using Core.DTOs.TeacherDashboardDTOs;
using Core.DTOs.TeacherDashboardDTOs.StudentsDTO;
using Core.Enums;
using Core.Models.Users;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Users
{
    public interface ITeacherRepository 
    {
        // Mahmoud / for TeacherDashboard
        Task<Teacher?> GetTeacherAsync(string teacherId);
        Task<List<CourseDashboardDTO>> GetTeacherCoursesAsync(Guid teacherId);
        Task<int> GetTotalStudentsAsync(Guid teacherId);
        Task<TeacherDashboardDTO> GetTeacherDashboardAsync(Guid teacherId);
        Task<TeacherInfoDTO> GetTeacherInfoAsync(Guid teacherId);
        Task<IdentityResult> UpdateTeacherInfoAsync(TeacherInfoDTO teacher,Guid teacherId);


        // Mahmoud / For TeacherDashboard - Students
         Task<StudentDetailsDTO> GetStudentDetailsAsync(Guid teacherId,Guid studentId);
         Task<int> TotalStudentOfCourse(Guid CourseId, Guid teacherId);
         Task<StudentListDTO> GetStudentsAsync(Guid teacherId,int pageNumber,int pageSize,string? searchQuery,Guid? courseId, string? status);
         Task<int> GetActiveStudentsAsync(Guid teacherId);
         Task<int> GetCompleteStudentsAsync(Guid teacherId);






        // Mohamd Haysam /for AdminDashboard
        //Task<Teacher> GetTeacherByIdAsync(string id);

        Task<IdentityResult> UpdateTeacherAsync(Teacher teacher);

        Task<IdentityResult> DeleteTeacherAsync(Teacher teacher);

        Task<IdentityResult> CreateTeacherAsync(Teacher teacher, string password);

        Task<List<Teacher>> GetAll();
        Task<int> GetTotalTeacherCountAsync();


    }
}
