using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Configuration;
using System.Text.RegularExpressions;

namespace WcfServiceLibrary1
{
   
    public class Service1 : IService1, IDisposable
    {
        private readonly SqlConnection connection;
        public Service1()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DataConnection"].ConnectionString;//Строка подключения App.config
            connection = new SqlConnection(connectionString);
            connection.Open();// открываем подключение
        }
        public int DeleteEmployee(int EmployerId)// метод удаления сотрудника по Id
        {
            string query = $"DELETE FROM Employees WHERE ID = {EmployerId}";// Запрос на удаление сотрудника по id
            SqlCommand myCommand = new SqlCommand(query, connection);
            int del = myCommand.ExecuteNonQuery(); 
            return del;// возвращает число удаленных сотрудников (1)
        }

        public void Dispose()
        {
            connection.Close();//закрываем подключение
        }

        public List<Employer> GetEmployees(string lastname, string firstname, string patronymic)// метод получения из Бд сотрудников
        {

            lastname = lastname.ToLower();
            firstname = firstname.ToLower();
            patronymic = patronymic.ToLower();//переводим в нижний регист для того чтобы невилировать регист введенный пользователем

            string query = "SELECT ID, LastName, FirstName, Patronymic, BirthDay FROM Employees";// Запрос на получение всех сотрудников
            SqlCommand command = new SqlCommand(query, connection);
            SqlDataReader dataReader = command.ExecuteReader();
            
            List<Employer> Employees = new List<Employer>();// список в котором мы будем хранить полученных выше сотрудников
            
            while (dataReader.Read())// перебираем всех сотрудников
            {
                Regex regex = new Regex(String.Format(@"^{0}\W*", lastname));// шаблон (регулярное выражение) для поиска успешных сопадений в поле LastName
                MatchCollection matches1 = regex.Matches(dataReader.GetString(1).ToLower());

                regex = new Regex(String.Format(@"^{0}\W*", firstname));// шаблон (регулярное выражение) для поиска успешных сопадений в поле FirstName
                MatchCollection matches2 = regex.Matches(dataReader.GetString(2).ToLower());

                regex = new Regex(String.Format(@"^{0}\W*", patronymic));// шаблон (регулярное выражение) для поиска успешных сопадений в поле Patronymic
                MatchCollection matches3 = regex.Matches(dataReader.GetString(3).ToLower());

                if (matches1.Count != 0 && matches2.Count != 0 && matches3.Count != 0)// условие которое срабатывает в случае успешных совпадений
                {
                    Employees.Add(new Employer()// добавляем в список отобранных сотрудников
                    {
                        ID = dataReader.GetInt32(0),
                        LastName = dataReader.GetString(1),
                        FirstName = dataReader.GetString(2),
                        Patronymic = dataReader.GetString(3),
                        BirthDay = dataReader.GetDateTime(4)
                    });
                }
            }
            return Employees;// возвращаем полученный список
        }

        public void SetEmploye(string lastname, string firstname, string patronymic, DateTime birthday)// метод для записи сотрудников в БД
        {
            string query = $"INSERT INTO Employees (LastName, FirstName, Patronymic, BirthDay) VALUES (N'{lastname}', N'{firstname}', N'{patronymic}', '{birthday.Year}.{birthday.Month}.{birthday.Day}')";// запрос на запись в таблицу сотрудников
            SqlCommand myCommand = new SqlCommand(query, connection);
            myCommand.ExecuteNonQuery();
        }
    }
}
