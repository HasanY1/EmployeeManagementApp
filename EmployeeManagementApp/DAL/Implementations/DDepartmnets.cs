using FinalProject.Models;
using log4net;

namespace EmployeeManagementApp.DAL.Implementations
{
    public class DDepartments : IDDepartmnets
    {
        private readonly EmployeeManagementDbContext _context;
        private static readonly ILog log = LogManager.GetLogger(typeof(DDepartments));

        public DDepartments(EmployeeManagementDbContext context)
        {
            _context = context;
        }

        public bool? CreateDepartment(Department? newDepartment)
        {
            try
            {
                if (newDepartment != null)
                {
                    _context.Departments?.Add(newDepartment);
                    _context.SaveChanges();
                    log.Info("Department created successfully.");
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.Error($"An error occurred while creating department: {ex.Message}");
                return false;
            }
            return false;
        }

        public bool? UpdateDepartment(Department? departmentToUpdate)
        {
            try
            {
                if (departmentToUpdate != null)
                {
                    var existingDepartment = _context.Departments?.FirstOrDefault(d => d.DepartmentID == departmentToUpdate.DepartmentID);

                    if (existingDepartment != null)
                    {
                        existingDepartment.DepartmentName = departmentToUpdate.DepartmentName;
                        _context.SaveChanges();
                        log.Info("Department updated successfully.");
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error($"An error occurred while updating department: {ex.Message}");
                return false;
            }
            return false;
        }

        public bool? DeleteDepartment(Guid? departmentId)
        {
            try
            {
                var departmentToDelete = _context.Departments?.FirstOrDefault(d => d.DepartmentID == departmentId);

                if (departmentToDelete != null)
                {
                    _context.Departments?.Remove(departmentToDelete);
                    _context.SaveChanges();
                    log.Info("Department deleted successfully.");
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.Error($"An error occurred while deleting department: {ex.Message}");
                return false;
            }
            return false;
        }

        public List<Department>? ViewAllDepartments()
        {
            try
            {
                return _context.Departments?.ToList();
            }
            catch (Exception ex)
            {
                log.Error($"An error occurred while viewing departments: {ex.Message}");
                return null;
            }
        }

        public Department? GetDepartmentById(Guid? departmentId)
        {
            try
            {
                if (departmentId != null)
                {
                    return _context.Departments?.FirstOrDefault(d => d.DepartmentID == departmentId);
                }
            }
            catch (Exception ex)
            {
                log.Error($"An error occurred while viewing department by id: {ex.Message}");
                return null;
            }
            return null;
        }
    }
}
