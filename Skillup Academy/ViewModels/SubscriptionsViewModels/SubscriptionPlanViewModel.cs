using Core.Models.Courses;
using Core.Models.Subscriptions;
using Core.Models.Users;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Skillup_Academy.ViewModels.SubscriptionsViewModels
{
    public class SubscriptionPlanViewModel
    { 
        // حالة الاشتراك
        public DateTime StartDate { get; set; }            // تاريخ البدء
        public DateTime EndDate { get; set; }              // تاريخ الانتهاء
        public bool IsActive { get; set; } = true;         // نشط/منتهي

        // الدفع
        public decimal PaidAmount { get; set; }            // المبلغ المدفوع
        public string? TransactionId { get; set; }          // رقم المعاملة
        public Guid SubscriptionId { get; set; }          // رقم المعاملة

        
    }
}
