using Core.Models.Courses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.TeacherDashboardDTOs
{
    public class TeacherDashboardDTO
    {
        public string TeacherName { get; set; }
        public string TeacherBio { get; set; }
        public string TeacherExpertise { get; set; }
       // public string TeacherEmail { get; set; }
        public string ProfilePictureUrl { get; set; }
        public Decimal TeacherRating { get; set; }

        public int TotalCourses { get; set; }
        public int TotalStudents { get; set; }
        public List<CourseDashboardDTO> Courses { get; set; }
    }
}
