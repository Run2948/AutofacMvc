using System.Linq;
using AutoFacMvc.Models;
using AutoFacMvc.Repository.Core;

namespace AutoFacMvc.Repository.Interface
{
    public interface IStudentRepository : IRepository<Student>
    {
        IQueryable<Student> GetIQueryableStudents();
    }
}