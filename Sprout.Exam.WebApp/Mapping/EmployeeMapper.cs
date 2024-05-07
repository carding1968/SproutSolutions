using AutoMapper;
using Sprout.Exam.Business.DataTransferObjects;
using Sprout.Exam.WebApp.Models;

namespace Sprout.Exam.WebApp.Mapping
{
    public class EmployeeMapper : Profile
    {
        public EmployeeMapper() {

            CreateMap<Employee, CreateEmployeeDto>();

            CreateMap<CreateEmployeeDto, Employee>();

            CreateMap<Employee, EditEmployeeDto>();

            CreateMap<EditEmployeeDto, Employee>();
        }
    }
}
