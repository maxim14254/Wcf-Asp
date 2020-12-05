using System;
using System.Collections.Generic;
using System.ServiceModel;


namespace WcfServiceLibrary1
{
    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени интерфейса "IService1" в коде и файле конфигурации.
    [ServiceContract]
    public interface IService1
    {
        [OperationContract]
        List<Employer> GetEmployees(string lastname, string firstname, string patronymic);

        [OperationContract]
        void SetEmploye(string lastname, string firstname, string patronymic, DateTime birthday);

        [OperationContract]
        int DeleteEmployee(int EmployerId);
    }

}
