using Core.Models.Lessons;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Configurations.Lessons
{
    public class LessonConfiguration : IEntityTypeConfiguration<Lesson>
    {
        public void Configure(EntityTypeBuilder<Lesson> builder)
        {
            // العلاقات  
            builder.HasOne(q => q.Course)
                .WithMany(e => e.Lessons)
                .HasForeignKey(q => q.CourseId)
                .OnDelete(DeleteBehavior.NoAction);

        }
    }
}