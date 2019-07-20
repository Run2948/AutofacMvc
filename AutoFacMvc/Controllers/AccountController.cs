using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoFacMvc.Common.Models;
using AutoFacMvc.Models.ViewModels;
using AutoFacMvc.Repository.Core;
using AutoFacMvc.Repository.Interface;

namespace AutoFacMvc.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IUnitOfWork _repositoryCenter;
        private readonly IUserRepository _userRepository;
        public AccountController(IUnitOfWork repositoryCenter)
        {
            _repositoryCenter = repositoryCenter;
            _userRepository = _repositoryCenter.GetRepository<IUserRepository>();
        }

        // GET: Account
        public ActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var loginUser = _userRepository.Login(model.UserName, model.Password);
                if (loginUser != null)
                    ModelState.AddModelError("error", "UserName or password is Incorrect.");
                Session[SystemKeys.UserSession] = loginUser;
                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }
    }
}