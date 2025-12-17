using Core.Models.Lessons;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.Lessons
{
    public class LessonBL
    {
        // استخدام Context مباشرةً (للتماشي مع النمط المطلوب)
        AppDbContext Context = new AppDbContext();

        public List<Lesson> GetAll()
        {
            // قد تحتاج هنا لعمل Include للعلاقات إذا كنت تستخدمها
            return Context.Lessons.ToList();
        }

        // ملاحظة: تم تعديل هذه الدالة لجلب العلاقات (مثل Course) لكي تعمل Details بشكل صحيح
        public Lesson GetById(Guid id)
        {
            return Context.Lessons
                //.Include(l => l.Course) // يفترض أن كيان الدرس Lesson يحتوي على خاصية Course
                .FirstOrDefault(c => c.Id == id);
        }

        // دالة إضافية لجلب الدروس لكورس معين (مفيد لدالة Index)
        public IQueryable<Lesson> Query()
        {
            return Context.Lessons.AsQueryable();
        }

        public void Add(Lesson lesson)
        {
            Context.Add(lesson);
            Context.SaveChanges();
        }

        // هذه الدالة ستعمل على حفظ التغييرات على الكيانات التي تم جلبها مسبقاً
        public void Update()
        {
            Context.SaveChanges();
        }

        public void Delete(Lesson lesson)
        {
            Context.Lessons.Remove(lesson);
            Context.SaveChanges();
        }

        public bool LessonExists(Guid id)
        {
            return Context.Lessons.Any(e => e.Id == id);
        }
    }
}
