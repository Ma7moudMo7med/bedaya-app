using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models.Subscriptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Subscriptions
{
	public class SubscriptionPlanConfiguration
	{
		public void Configure(EntityTypeBuilder<SubscriptionPlan> builder)
		{ 
			// العلاقات
			builder.HasOne(s => s.User)
				.WithMany(u => u.Subscribes)
				.HasForeignKey(s => s.UserId)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasOne(s => s.Subscription)
				.WithMany(s => s.Subscribes)
				.HasForeignKey(s => s.SubscriptionId)
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasOne(s => s.Course)
				.WithMany(c => c.Subscribes)
				.HasForeignKey(s => s.CourseId)
				.OnDelete(DeleteBehavior.Cascade);
		}
	}
}
