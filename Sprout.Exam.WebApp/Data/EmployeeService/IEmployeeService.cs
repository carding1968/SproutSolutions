using Sprout.Exam.Business.DataTransferObjects;
using Sprout.Exam.WebApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sprout.Exam.WebApp.Data.EmployeeService
{
    public interface IEmployeeService
    {
        Task<Employee> CreateEmployee(Employee employee);


        
        Task<List<EmployeeDto>> GetEmployees();


        Task<bool> Delete(int id);

        Employee GetById(int id);

        Task<Employee> GetId(int id);

        Task<Employee> EditEmployee(Employee employee);
    }
}
