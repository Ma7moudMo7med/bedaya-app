using Core.Interfaces.Exams;
using Core.Models.Exams;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Exams
{
    public class ExamAttemptRepository : IExamAttemptRepository
    {
        AppDbContext context;
        public ExamAttemptRepository(AppDbContext _context)
        {
            context = _context;
        }
        //basic CRUD
        public void ExamAttemptAdd(ExamAttempt examattemp)
        {
            context.ExamAttempts.Add(examattemp);
        }
        public void Update(ExamAttempt examAttempt)
        {
            context.Update(examAttempt);
        }
        public void DeleteFromDB(ExamAttempt examattemp)
        {
            context.ExamAttempts.Remove(examattemp);
        }
        public List<ExamAttempt> ShowAll()
        {
            return context.ExamAttempts.ToList();
        }
        public ExamAttempt ShowDetails(Guid id)
        {
            return context.ExamAttempts.FirstOrDefault(E => E.Id == id);
        }
        public void Save()
        {
            context.SaveChanges();
        }

    }
}
