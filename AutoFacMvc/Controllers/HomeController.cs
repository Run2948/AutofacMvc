using System.Linq;
using System.Web.Mvc;
using AutoFacMvc.Models;
using AutoFacMvc.Repository;
using AutoFacMvc.Repository.Core;

namespace AutoFacMvc.Controllers
{
    /// <summary>
    /// MiniProfiler监控EF与.NET MVC项目 https://blog.csdn.net/zhangwenting6/article/details/78793994
    /// </summary>
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _repositoryCenter;
        private readonly IStudentRepository _studentRepository;
        public HomeController(IUnitOfWork repositoryCenter)
        {
            _repositoryCenter = repositoryCenter;
            _studentRepository = _repositoryCenter.GetRepository<IStudentRepository>();
        }

        public ActionResult Index(Student sessionStudent)
        {
            //Repository使用IEnumerable返回结果
            //var students = _studentRepository.GetIEnumerableStudents().ToList();
            //if (sessionStudent != null)
            //{
            //    students.Add(sessionStudent);
            //}
            //Repository使用IQueryable返回结果
            var students = _studentRepository.GetIQueryableStudents().ToList();
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

        [HttpGet]
        public ActionResult AddNewStudent()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddNewStudent(Student student)
        {
            _studentRepository.Create(student);
            _repositoryCenter.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
