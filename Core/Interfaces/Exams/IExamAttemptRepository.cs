using Core.Models.Exams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Exams
{
    public interface IExamAttemptRepository
    {
        public void ExamAttemptAdd(ExamAttempt examattemp);
        public void Update(ExamAttempt examAttempt);
        public void DeleteFromDB(ExamAttempt examattemp);
        public List<ExamAttempt> ShowAll();
        public ExamAttempt ShowDetails(Guid id);
        public void Save();
    }
}
