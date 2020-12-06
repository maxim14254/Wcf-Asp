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
        public ActionResult Index()// Главная стрвница
        {
            var service = new Service1Client();
            List<Employer> Employees = service.GetEmployees("", "", "").ToList<Employer>();// получаем список всех сотрудников
            return View(Employees);// возвращаем модель
        }

        [HttpGet]
        public ActionResult Search()// страница поиска сторудников
        {
            return View();
        }

        [HttpPost]
        public ActionResult GoSearch(string firstname, string lastname, string patronymic)// странца вывода результата поиска сотрудников
        {
            var service = new Service1Client();
            List<Employer> Employees = service.GetEmployees(lastname, firstname, patronymic).ToList<Employer>();// получаем список сотрудников по определенным критериям
            return View(Employees);// возвращаем модель
        }
        
        [HttpGet]
        public ActionResult Error()// странца ошибки 404
        {
            Response.StatusCode = 404;
            return View();
        }

        [HttpGet]
        public ActionResult Delete(string ID)// запрос на удаление сотрудника
        {
            ActionResult actionResult;

            try// пробуем перевести из string в int ID
            {
                int id = int.Parse(ID);
                
                var service = new Service1Client();
                int del = service.DeleteEmployee(id);// получаем кол-во (1) удаленных сотрудников
                if (del != 0)// если сотрудника по данному id нет то выводим 404
                {
                    actionResult = RedirectToAction("Index", "Home");// переходим на главную страницу
                }
                else { actionResult = RedirectToAction("Error", "Home"); }
            }
            catch (System.FormatException) // если в id записаны буквы то выводим 404
            {
                actionResult = RedirectToAction("Error", "Home");
            }
            return actionResult;// вызвращаем полученный результат (страницу)
        }

        [HttpGet]
        public ActionResult SetEmploye()// страница добавления сотрудника
        {
            return View();
        }

        [HttpPost]
        public ActionResult GoSetEmploye(string firstname, string lastname, string patronymic, DateTime birthday)// запрос на добавление сотрудника
        {
            if (firstname != null && lastname != null && patronymic != null && birthday != null)// если паметры пустые то выводи 404
            {
                var service = new Service1Client();
                service.SetEmploye(lastname, firstname, patronymic, birthday);// сохраняем сотрудника в БД
                return RedirectToAction("Index", "Home");// переходим на главную страницу
            }
            return RedirectToAction("Error", "Home");
        }
    }
}