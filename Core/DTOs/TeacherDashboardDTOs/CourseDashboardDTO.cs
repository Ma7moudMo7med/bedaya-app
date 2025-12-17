using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.TeacherDashboardDTOs
{
    public class CourseDashboardDTO
    {
        public Guid CourseId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsPublished { get; set; }
        public bool IsFree { get; set; }
        public string? ThumbnailUrl { get; set; }

        // public bool IsDeleted { get; set; } = false;
        public DateTime CreatedDate { get; set; }
        public int TotalDuration { get; set; }
        public double AverageRating { get; set; }
        public int TotalLessons { get; set; }
        public int TotalStudents { get; set; }

        public Guid teacherId { get; set; }

    }
}
