using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Sprout.Exam.Business.DataTransferObjects;
using Sprout.Exam.Common.Enums;
using AutoMapper;
using Sprout.Exam.WebApp.Data.EmployeeService;
using Sprout.Exam.WebApp.Models;
using System.Runtime.CompilerServices;

namespace Sprout.Exam.WebApp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly IMapper _mapper;

        public EmployeesController(IEmployeeService employeeService, IMapper mapper) {
            _employeeService = employeeService;
            _mapper = mapper;
        }

        /// <summary>
        /// Refactor this method to go through proper layers and fetch from the DB.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> List()
        {
            var result = await _employeeService.GetEmployees();

            
            

            //var result = await Task.FromResult(StaticEmployees.ResultList);
            return Ok(result);
        }

        /// <summary>
        /// Refactor this method to go through proper layers and fetch from the DB.
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _employeeService.GetId(id);
            
            

            //var result = await Task.FromResult(StaticEmployees.ResultList.FirstOrDefault(m => m.Id == id));
            return Ok(result);
        }

        /// <summary>
        /// Refactor this method to go through proper layers and update changes to the DB.
        /// </summary>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(EditEmployeeDto input)
        {
            var item = await _employeeService.EditEmployee(_mapper.Map<Employee>(input));

            //var item = await Task.FromResult(StaticEmployees.ResultList.FirstOrDefault(m => m.Id == input.Id));
            if (item == null) return NotFound();
            item.FullName = input.FullName;
            item.Tin = input.Tin;
            item.Birthdate = input.Birthdate;
            item.EmployeeTypeId = input.EmployeeTypeId;
            return Ok(item);
        }

        /// <summary>
        /// Refactor this method to go through proper layers and insert employees to the DB.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post(CreateEmployeeDto input)
        {

            var result = await _employeeService.CreateEmployee(_mapper.Map<Employee>(input));

            
            var output = _mapper.Map<CreateEmployeeDto>(result);
            
           

            
            return Created($"/api/employees/{output.Id}", output.Id);
            





            /*var id = await Task.FromResult(StaticEmployees.ResultList.Max(m => m.Id) + 1);

             StaticEmployees.ResultList.Add(new EmployeeDto
             {
                 Birthdate = input.Birthdate.ToString("yyyy-MM-dd"),
                 FullName = input.FullName,
                 Id = id,
                 Tin = input.Tin,
                 TypeId = input.TypeId
             });*/

           
        }


        /// <summary>
        /// Refactor this method to go through proper layers and perform soft deletion of an employee to the DB.
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _employeeService.Delete(id);

            //var result = await Task.FromResult(StaticEmployees.ResultList.FirstOrDefault(m => m.Id == id));
            if (!result) return NotFound();
            //StaticEmployees.ResultList.RemoveAll(m => m.Id == id);
            return Ok(id);
        }



        /// <summary>
        /// Refactor this method to go through proper layers and use Factory pattern
        /// </summary>
        /// <param name="id"></param>
        /// <param name="absentDays"></param>
        /// <param name="workedDays"></param>
        /// <returns></returns>
        /// <returns></returns>
        [HttpPost("{id}/calculate")]
        
        public async Task<IActionResult> Calculate(CalculateSalaryDto input)
        {
            //var result = await Task.FromResult(StaticEmployees.ResultList.FirstOrDefault(m => m.Id == id));
            var result = await _employeeService.GetId(input.Id);

            if (result == null) return NotFound();
            var type = (EmployeeType) result.EmployeeTypeId;
            
            return type switch
            {
                EmployeeType.Regular =>
                    Ok(ComputeSalary(result.EmployeeTypeId, input.AbsentDays, input.WorkedDays)),
                EmployeeType.Contractual =>
                    //create computation for contractual.
                    Ok(ComputeSalary(result.EmployeeTypeId, input.AbsentDays, input.WorkedDays)),
                _ => NotFound("Employee Type not found")
            }; ;

        }

        public string ComputeSalary(int employeeTypeId, decimal absent, decimal workedDays) {
            decimal salary = 0;

            var type = (EmployeeType)employeeTypeId;

            switch (type) {
                case EmployeeType.Regular:
                    decimal basicSalary = 20000;
                    decimal absentComp = (basicSalary / 22) * absent;
                    decimal tax = Math.Truncate(basicSalary * (decimal)0.12);
                    salary = basicSalary - absentComp - tax;

                    break;
                case EmployeeType.Contractual:
                    decimal dayRate = 500;
                    salary = dayRate * workedDays;


                    break;


            }

         
            
            return salary.ToString("0.00");

        }

       

    }
}
