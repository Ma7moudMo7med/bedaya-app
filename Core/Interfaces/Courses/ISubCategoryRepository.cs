using Core.Models.Courses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Courses
{
    public interface ISubCategoryRepository
    {
        public List<SubCategory> GetAll();
        public void Add(SubCategory SubCategory);
        public SubCategory GetById(Guid Id);
        public void Delete(SubCategory SubCategory);

        public void Update(SubCategory SubCategory);
        public void Save();
        Task<int> GetTotalSubCategoryCountAsync();

    }
}
