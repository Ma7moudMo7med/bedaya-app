using Infrastructure.Data;
using Core.Models.Exams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.Exams
{
    public class ExamBL
    {
        AppDbContext context = new AppDbContext();
        public List<Exam> ShowAll()
        {
            return context.Exams.ToList();
        }

        public Exam ShowDetails(Guid id)
        {
            return context.Exams.FirstOrDefault(E => E.Id == id);
        }

        public void ExamAdd(Exam exam)
        {
            context.Exams.Add(exam);

            context.SaveChanges();
        }

        public void SaveInDB()
        {
            context.SaveChanges();
        }

        public void DeleteFromDB(Exam exam)
        {
            context.Exams.Remove(exam);

            context.SaveChanges();
        }
    }
}
