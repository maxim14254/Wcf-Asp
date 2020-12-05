using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication3.ServiceReference1;

namespace WebApplication3.Controllers
{
    public class HomeController : Controller
    {

        [HttpGet]
        public ActionResult Index()
        {
            var service = new Service1Client();
            List<Employer> Employees = service.GetEmployees("", "", "").ToList<Employer>();
            return View(Employees);
        }

        [HttpGet]
        public ActionResult Search()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GoSearch(string firstname, string lastname, string patronymic)
        {
            var service = new Service1Client();
            List<Employer> Employees = service.GetEmployees(lastname, firstname, patronymic).ToList<Employer>();
            return View(Employees);
        }

        public ActionResult Error()
        {
            Response.StatusCode = 404;
            return View();
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var service = new Service1Client();
            int del = service.DeleteEmployee(id);
            if (del != 0)
            {
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Error", "Home");
        }

        [HttpGet]
        public ActionResult SetEmploye()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GoSetEmploye(string firstname, string lastname, string patronymic, DateTime birthday)
        {
            if (firstname != null && lastname != null && patronymic != null && birthday != null)
            {
                var service = new Service1Client();
                service.SetEmploye(lastname, firstname, patronymic, birthday);
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Error", "Home");
        }
    }
}