using System.Collections.Generic;
using System.Linq;
using AutoFacMvc.Models;
using AutoFacMvc.Repository.Core;

namespace AutoFacMvc.Repository
{
    public interface IStudentRepository : IRepository<Student>
    {
        IEnumerable<dynamic> GetIEnumerableStudents();
        IQueryable<dynamic> GetIQueryableStudents();
    }
}