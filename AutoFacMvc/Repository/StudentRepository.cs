using System.Collections.Generic;
using System.Linq;
using AutoFacMvc.Models;
using AutoFacMvc.Repository.Core;
using AutoFacMvc.Repository.Interface;

namespace AutoFacMvc.Repository
{
    public class StudentRepository : BaseRepository<Student>, IStudentRepository
    {
        private readonly SchoolContext _context;

        public StudentRepository(SchoolContext context)
            : base(context)
        {
            _context = context;
        }

        public IQueryable<Student> GetIQueryableStudents()
        {
            return _context.Students;
        }
    }
}