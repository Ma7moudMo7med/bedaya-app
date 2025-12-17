using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Users
{
	public class TeacherConfiguration : IEntityTypeConfiguration<Teacher>
	{
		public void Configure(EntityTypeBuilder<Teacher> builder)
		{
			// العلاقات
			builder.HasMany(t => t.Courses)
				.WithOne(c => c.Teacher)
				.HasForeignKey(c => c.TeacherId)
				.OnDelete(DeleteBehavior.Restrict);
		}
	}
}
