using System.Collections.Generic;
using System.Linq;
using AutoFacMvc.Models;
using AutoFacMvc.Repository.Core;

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

        public IEnumerable<dynamic> GetIEnumerableStudents()
        {
            return _context.Students;
        }

        public IQueryable<dynamic> GetIQueryableStudents()
        {
            return _context.Students;
        }
    }
}