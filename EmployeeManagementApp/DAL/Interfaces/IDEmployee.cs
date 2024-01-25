using FinalProject.Models;

namespace WebApplication2.Dal.Implementations
{
    public interface IDEmployee
    {
        public bool? CreateEmployee(Employee? newEmployee);
        public bool? DeleteEmployee(Guid? employeeId);
        public Employee? GetEmployeeById(Guid? employeeId);
        public List<Employee>? GetEmployeesByDepartmentId(Guid? departmentId);
        public bool? UpdateEmployee(Employee? employeeToUpdate);
        public List<Employee>? ViewAllEmployees();
    }
}