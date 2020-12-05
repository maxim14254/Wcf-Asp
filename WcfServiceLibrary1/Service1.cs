using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Configuration;
using System.Text.RegularExpressions;

namespace WcfServiceLibrary1
{
    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени класса "Service1" в коде и файле конфигурации.
    public class Service1 : IService1, IDisposable
    {
        private readonly SqlConnection connection;
        public Service1()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DataConnection"].ConnectionString;
            connection = new SqlConnection(connectionString);
            connection.Open();
        }
        public int DeleteEmployee(int EmployerId)
        {
            string query = $"DELETE FROM Employees WHERE ID = {EmployerId}";
            SqlCommand myCommand = new SqlCommand(query, connection);
            int del = myCommand.ExecuteNonQuery();
            return del;
        }

        public void Dispose()
        {
            connection.Close();
        }

        public List<Employer> GetEmployees(string lastname, string firstname, string patronymic)
        {

            lastname = lastname.ToLower();
            firstname = firstname.ToLower();
            patronymic = patronymic.ToLower();

            string query = "SELECT ID, LastName, FirstName, Patronymic, BirthDay FROM Employees";
            SqlCommand command = new SqlCommand(query, connection);
            SqlDataReader dataReader = command.ExecuteReader();
            List<Employer> Employees = new List<Employer>();
            while (dataReader.Read())
            {
                Regex regex = new Regex(String.Format(@"^{0}\W*", lastname));
                MatchCollection matches1 = regex.Matches(dataReader.GetString(1).ToLower());

                regex = new Regex(String.Format(@"^{0}\W*", firstname));
                MatchCollection matches2 = regex.Matches(dataReader.GetString(2).ToLower());

                regex = new Regex(String.Format(@"^{0}\W*", patronymic));
                MatchCollection matches3 = regex.Matches(dataReader.GetString(3).ToLower());

                if (matches1.Count != 0 && matches2.Count != 0 && matches3.Count != 0)
                {
                    Employees.Add(new Employer()
                    {
                        ID = dataReader.GetInt32(0),
                        LastName = dataReader.GetString(1),
                        FirstName = dataReader.GetString(2),
                        Patronymic = dataReader.GetString(3),
                        BirthDay = dataReader.GetDateTime(4)
                    });
                }
            }
            return Employees;
        }

        public void SetEmploye(string lastname, string firstname, string patronymic, DateTime birthday)
        {
            string query = $"INSERT INTO Employees (LastName, FirstName, Patronymic, BirthDay) VALUES (N'{lastname}', N'{firstname}', N'{patronymic}', '{birthday.Year}.{birthday.Month}.{birthday.Day}')";
            SqlCommand myCommand = new SqlCommand(query, connection);
            myCommand.ExecuteNonQuery();
        }
    }
}
