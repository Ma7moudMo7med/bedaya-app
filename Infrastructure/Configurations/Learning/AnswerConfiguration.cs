using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models.Learning;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Learning
{
	public class AnswerConfiguration : IEntityTypeConfiguration<Answer>
	{
		public void Configure(EntityTypeBuilder<Answer> builder)
		{ 
			// العلاقات
			builder.HasOne(a => a.Question)
				.WithMany(q => q.Answers)
				.HasForeignKey(a => a.QuestionId)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasOne(a => a.User)
				.WithMany(u => u.Answers)
				.HasForeignKey(a => a.UserId)
				.OnDelete(DeleteBehavior.Restrict);
		}
	}
}
