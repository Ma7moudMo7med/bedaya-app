using Infrastructure.Data;
using Core.Models.Exams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.Exams
{
    public class ExamAttemptBL
    {

        AppDbContext context = new AppDbContext();
        public List<ExamAttempt> ShowAll()
        {
            return context.ExamAttempts.ToList();
        }
        public ExamAttempt ShowDetails(Guid id)
        {
            return context.ExamAttempts.FirstOrDefault(E => E.Id == id);
        }

        public void ExamAttemptAdd(ExamAttempt examattemp)
        {
            context.ExamAttempts.Add(examattemp);

            context.SaveChanges();
        }

        public void SaveInDB()
        {
            context.SaveChanges();
        }

        public void DeleteFromDB(ExamAttempt examattemp)
        {
            context.ExamAttempts.Remove(examattemp);

            context.SaveChanges();
        }
    }
}
