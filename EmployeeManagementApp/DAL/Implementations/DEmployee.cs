using FinalProject.Models;
using log4net;

namespace WebApplication2.Dal.Implementations
{
    public class DEmployee : IDEmployee
    {
        private readonly EmployeeManagementDbContext _context;
        private static readonly ILog log = LogManager.GetLogger(typeof(DEmployee));

        public DEmployee(EmployeeManagementDbContext context)
        {
            _context = context;
        }

        public bool? UpdateEmployee(Employee? employeeToUpdate)
        {
            try
            {
                if (employeeToUpdate != null)
                {
                    var existingEmployee = _context.Employees?.FirstOrDefault(e => e.EmployeeID == employeeToUpdate.EmployeeID);
                    if (existingEmployee != null)
                    {
                        existingEmployee.Name = employeeToUpdate.Name;
                        existingEmployee.ContactInfo = employeeToUpdate.ContactInfo;
                        existingEmployee.Role = employeeToUpdate.Role;
                        existingEmployee.DepartmentID = employeeToUpdate.DepartmentID;

                        _context.SaveChanges();
                        log.Info("Employee updated successfully.");
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error($"An error occurred while updating employee: {ex.Message}");
                return false;
            }
            return false;
        }

        public bool? DeleteEmployee(Guid? employeeId)
        {
            try
            {
                var employeeToDelete = _context.Employees?.FirstOrDefault(e => e.EmployeeID == employeeId);

                if (employeeToDelete != null)
                {
                    _context.Employees?.Remove(employeeToDelete);
                    _context.SaveChanges();
                    log.Info("Employee deleted successfully.");
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.Error($"An error occurred while deleting employee: {ex.Message}");
                return false;
            }
            return false;
        }

        public bool? CreateEmployee(Employee? newEmployee)
        {
            try
            {
                if (newEmployee != null)
                {
                    _context.Employees?.Add(newEmployee);
                    _context.SaveChanges();
                    log.Info("Employee created successfully.");
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.Error($"An error occurred while creating employee: {ex.Message}");
                return false;
            }
            return false;
        }

        public List<Employee>? GetEmployeesByDepartmentId(Guid? departmentId)
        {
            try
            {
                if (departmentId != null)
                {
                    return _context.Employees?.Where(e => e.DepartmentID == departmentId).ToList();
                }
            }
            catch (Exception ex)
            {
                log.Error($"An error occurred while getting employees in department: {ex.Message}");
                return null;
            }
            return null;
        }

        public Employee? GetEmployeeById(Guid? employeeId)
        {
            try
            {
                if (employeeId != null)
                {
                    return _context.Employees?.FirstOrDefault(e => e.EmployeeID == employeeId);
                }
            }
            catch (Exception ex)
            {
                log.Error($"An error occurred while getting employee by id: {ex.Message}");
                return null;
            }
            return null;
        }

        public List<Employee>? ViewAllEmployees()
        {
            try
            {
                return _context.Employees?.ToList();
            }
            catch (Exception ex)
            {
                log.Error($"An error occurred while getting all employees: {ex.Message}");
                return null;
            }
        }
    }
}
