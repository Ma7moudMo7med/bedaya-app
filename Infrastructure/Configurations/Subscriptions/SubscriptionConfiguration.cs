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
	public class SubscriptionConfiguration : IEntityTypeConfiguration<Subscription>
	{
		public void Configure(EntityTypeBuilder<Subscription> builder)
		{
			// العلاقات
			builder.HasMany(s => s.Subscribes)
				.WithOne(s => s.Subscription)
				.HasForeignKey(s => s.SubscriptionId)
				.OnDelete(DeleteBehavior.Restrict);
		}
	}
}
