using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WcfServiceLibrary1
{
    [DataContract]
    public class Employer//класс сотрудников
    {
        [DataMember]
        public int ID { get; set; }

        [DataMember]
        public string LastName { get; set; }

        [DataMember]
        public string FirstName { get; set; }

        [DataMember]
        public string Patronymic { get; set; }

        [DataMember]
        public DateTime BirthDay { get; set; }

        [DataMember]
        public int Age// вычисляется автоматически
        {
            get
            {
                int age;
                if (BirthDay.DayOfYear <= DateTime.Now.DayOfYear)// сравниваем номер дня рождения в году и номера сегодняшнего дня в году 
                {
                    age = DateTime.Now.Year - BirthDay.Year;
                }
                else
                {
                    age = DateTime.Now.Year - BirthDay.Year - 1;
                }
                return age;
            }
            set { }
        }
    }
}
