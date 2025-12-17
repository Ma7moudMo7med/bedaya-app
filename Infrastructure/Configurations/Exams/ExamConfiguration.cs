using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models.Exams;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Exams
{
	public class ExamConfiguration : IEntityTypeConfiguration<Exam>
	{
		public void Configure(EntityTypeBuilder<Exam> builder)
		{ 
			// العلاقات
			builder.HasOne(e => e.Course)
				.WithMany(c => c.Exams)
				.HasForeignKey(e => e.CourseId)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(e => e.Questions)
				.WithOne(q => q.Exam)
				.HasForeignKey(q => q.ExamId)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(e => e.ExamAttempts)
				.WithOne(ea => ea.Exam)
				.HasForeignKey(ea => ea.ExamId)
				.OnDelete(DeleteBehavior.Cascade);
		}
	}

}
