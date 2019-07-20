using AutoFacMvc.Common.Models;
using AutoFacMvc.Models;
using AutoFacMvc.Repository;
using AutoFacMvc.Repository.Core;
using System.Linq;
using System.Web.Mvc;
using AutoFacMvc.Common.Logging;
using AutoFacMvc.Repository.Interface;

namespace AutoFacMvc.Controllers
{
    public class HomeController : UserBaseController
    {
        private readonly IUnitOfWork _repositoryCenter;
        private readonly IStudentRepository _studentRepository;
        public HomeController(IUnitOfWork repositoryCenter)
        {
            _repositoryCenter = repositoryCenter;
            _studentRepository = _repositoryCenter.GetRepository<IStudentRepository>();
        }

        public ActionResult Index(SessionInfo loginUser)
        {
            //Repository使用IQueryable返回结果
            var students = _studentRepository.GetIQueryableStudents().ToList();
            LogManager.Info(GetType(), $"{loginUser.UserName}用户访问了学生列表");
            return View(students);
        }

        [HttpGet]
        public ActionResult EditStudent(int? id)
        {
            var student = new Student();
            if (id != null && id > 0)
                student = _studentRepository.Find(id);
            return View(student);
        }

        [HttpPost]
        public ActionResult EditStudent(Student student, SessionInfo loginUser)
        {
            student.CreatorId = loginUser.Id;
            student.CreatorName = loginUser.RealName;
            if (student.Id > 0)
                _studentRepository.Update(student);
            else
                _studentRepository.Create(student);
            _repositoryCenter.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public ActionResult DeleteStudent(int? id)
        {
            if (id != null && id > 0)
            {
                _studentRepository.Delete(l => l.Id == id);
                _repositoryCenter.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
