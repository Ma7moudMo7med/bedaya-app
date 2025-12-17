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
	public class StudentConfiguration : IEntityTypeConfiguration<Student>
	{
		public void Configure(EntityTypeBuilder<Student> builder)
		{ 
			// العلاقات
			builder.HasMany(s => s.ExamAttempt)
				.WithOne(e => e.Student)
				.HasForeignKey(e => e.StudentId)
				.OnDelete(DeleteBehavior.Cascade);
		}
	}

}
