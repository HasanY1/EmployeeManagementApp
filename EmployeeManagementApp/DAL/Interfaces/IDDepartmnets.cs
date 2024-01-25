using FinalProject.Models;

namespace EmployeeManagementApp.DAL.Implementations
{
    public interface IDDepartmnets
    {
        bool? CreateDepartment(Department? newDepartment);
        bool? DeleteDepartment(Guid? departmentId);
        Department? GetDepartmentById(Guid? departmentId);
        bool? UpdateDepartment(Department? departmentToUpdate);
        List<Department>? ViewAllDepartments();
    }
}