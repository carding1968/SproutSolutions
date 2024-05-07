using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Sprout.Exam.Business.DataTransferObjects;
using Sprout.Exam.WebApp.Models;

namespace Sprout.Exam.WebApp.Data.EmployeeService
{
    public class EmployeeService : IEmployeeService
    {
        private readonly ApplicationDbContext _appDbContext;

        public EmployeeService(ApplicationDbContext appDbContext) {
            _appDbContext = appDbContext;
        }


        public async Task<bool> Delete(int id)
        {
            var employee = GetById(id);
            if (employee is not null)
            {
                _appDbContext.Employee.Remove(employee);
                await _appDbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<Employee> EditEmployee(Employee employee)
        {
            var employeeDetail = GetById(employee.Id);
            if (employee is not null)
            {
               
                employeeDetail.FullName = employee.FullName;
                employeeDetail.Birthdate = employee.Birthdate;
                employeeDetail.EmployeeTypeId = employee.EmployeeTypeId;
                employeeDetail.Tin = employee.Tin;


               
                await _appDbContext.SaveChangesAsync();
            }
            return employeeDetail;
        }

        public Employee GetById(int id)
        {
            return _appDbContext.Employee.FirstOrDefault(m => m.Id == id);
        }

        public async Task<Employee> GetId(int id)
        {
            return await _appDbContext.Employee.FirstOrDefaultAsync(m => m.Id == id);
        }

        // List of all employee
        public async Task<List<EmployeeDto>> GetEmployees()
        {
            List<Employee> query = await _appDbContext.Employee.OrderByDescending(m => m.Id).ToListAsync();

            List<EmployeeDto> result = new List<EmployeeDto>();

            result = query.Select(
                            x => new EmployeeDto()
                            {
                                Id = x.Id,
                                FullName = x.FullName,
                                Tin = x.Tin,
                                Birthdate = x.Birthdate.ToString("yyyy-MM-dd"),
                                EmployeeTypeId = x.EmployeeTypeId
                            }
                            ).ToList();

            return result;
        }

        // Add new employee
        public async Task<Employee> CreateEmployee(Employee employee)
        {
            try {
                if (employee is not null)
                {
                   
                    await _appDbContext.Employee.AddAsync(employee);
                    await _appDbContext.SaveChangesAsync();


                }
                
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            return employee;
        }

    }
}
