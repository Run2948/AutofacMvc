using System.Linq;
using System.Web.Mvc;
using AutoFacMvc.Models;
using AutoFacMvc.Repository;
using AutoFacMvc.Repository.Core;

namespace AutoFacMvc.Controllers
{
    /// <summary>
    /// https://miniprofiler.com/dotnet/AspDotNet
    /// </summary>
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWorkRepository;
        private readonly IStudentRepository _studentRepository;
        public HomeController(IUnitOfWork unitOfWorkRepository)
        {
            _unitOfWorkRepository = unitOfWorkRepository;
            _studentRepository = unitOfWorkRepository.GetRepository<IStudentRepository>();
        }

        public ActionResult Index(Student sessionStudent)
        {
            //Repository使用IEnumerable返回结果
            var students = _studentRepository.GetIEnumerableStudents().Take(2).ToList();
            if (sessionStudent != null)
            {
                students.Add(sessionStudent);
            }
            //Repository使用IQueryable返回结果
            //var students = _studentRepository.GetIQueryableStudents().Take(2);
            return View(students);
        }

        public ActionResult SetSession()
        {
            var student = new Student
            {
                Age = 18,
                Id = 13,
                Name = "Tester"
            };
            Session["student"] = student;
            return new ContentResult { Content = "Add student in session" };
        }

        public ActionResult AddNewStudent()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddNewStudent(Student student)
        {
            _studentRepository.Create(student);
            _unitOfWorkRepository.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
