using Core.Models.Courses;
using Core.Models.Users;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Skillup_Academy.ViewModels.CourseReviewViewModel
{
    public class CourseReviewViewModel
    {
        public Guid Id { get; set; }                       // المعرف الفريد للتقييم
       
        public int? Rating { get; set; }                    // التقييم (1-5)
        [Required(ErrorMessage = "Required Feild")]

        public string Comment { get; set; }                // التعليق
        public DateTime ReviewDate { get; set; } = DateTime.Now; // تاريخ التقييم

        // التقييم التفصيلي
        [Required(ErrorMessage = "Required Feild")]
        [Range(1,5, ErrorMessage = "Enter valid Rating from 1 to 5")]

        public int ContentRating { get; set; }             // تقييم المحتوى
        [Required(ErrorMessage = "Required Feild")]
        [Range(1, 5, ErrorMessage = "Enter valid Rating from 1 to 5")]

        public int TeachingRating { get; set; }            // تقييم الشرح
        [Required(ErrorMessage = "Required Feild")]

        // الموافقة
        public bool IsApproved { get; set; } = false;      // موافق عليه/غير موافق


        // العلاقات	
        public Guid CourseId { get; set; }                 // الكورس
        //public SelectList? Courses { get; set; }                 // الكورس

        //public Guid? UserId { get; set; }                 // المستخدم
        //public SelectList? Users { get; set; }                     // المستخدم
    }
}
