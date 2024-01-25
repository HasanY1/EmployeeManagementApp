using FinalProject.Models;
using log4net;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementApp.DAL.Implementations
{
    public class DSalaryDetail : IDSalaryDetail
    {
        private readonly EmployeeManagementDbContext _context;
        private static readonly ILog log = LogManager.GetLogger(typeof(DSalaryDetail));

        public DSalaryDetail(EmployeeManagementDbContext context)
        {
            _context = context;
        }

        public bool? CreateSalaryDetail(SalaryDetail? newSalaryDetail)
        {
            try
            {
                if (newSalaryDetail != null)
                {
                    _context.SalaryDetails?.Add(newSalaryDetail);
                    _context.SaveChanges();
                    log.Info("Salary detail created successfully.");
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.Error($"An error occurred while creating salary detail: {ex.Message}");
                return false;
            }
            return false;
        }

        public bool? UpdateSalaryDetail(SalaryDetail? updatedSalaryDetail)
        {
            try
            {
                if (updatedSalaryDetail != null)
                {
                    _context.SalaryDetails?.Update(updatedSalaryDetail);
                    _context.SaveChanges();
                    log.Info("Salary detail updated successfully.");
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.Error($"An error occurred while updating salary detail: {ex.Message}");
                return false;
            }
            return false;
        }

        public List<SalaryDetail>? GetSalaryDetailsByEmployeeId(Guid? employeeId)
        {
            try
            {
                if (employeeId != null)
                {
                    return _context.SalaryDetails?.Where(s => s.EmployeeID == employeeId).ToList();
                }
            }
            catch (Exception ex)
            {
                log.Error($"An error occurred while getting salary detail for employee: {ex.Message}");
                return null;
            }
            return null;
        }

        public List<SalaryDetail>? GetSalaryDetailsByDepartmentId(Guid? departmentId)
        {
            try
            {
                if (departmentId != null)
                {
                    return _context.SalaryDetails?
                        .Include(sd => sd.Employee)
                        .Where(sd => sd.Employee != null && sd.Employee.DepartmentID == departmentId)
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                log.Error($"An error occurred while getting salary detail for employees in department: {ex.Message}");
                return null;
            }
            return null;
        }

        public SalaryDetail? GetSalaryDetailById(Guid salaryId)
        {
            try
            {
                if (_context.SalaryDetails != null)
                {
                    return _context.SalaryDetails.FirstOrDefault(sd => sd.SalaryID == salaryId);
                }
                return null;
            }
            catch (Exception ex)
            {
                log.Error($"An error occurred while retrieving salary details by ID: {ex.Message}");
                return null;
            }
        }

        public bool DeleteSalaryDetail(Guid salaryId)
        {
            try
            {
                if (_context.SalaryDetails != null)
                {
                    var salaryDetailToDelete = _context.SalaryDetails.Find(salaryId);
                    if (salaryDetailToDelete != null)
                    {
                        _context.SalaryDetails.Remove(salaryDetailToDelete);
                        _context.SaveChanges();
                        log.Info("Salary detail deleted successfully.");
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                log.Error($"An error occurred while deleting salary details: {ex.Message}");
                return false;
            }
        }

        public List<SalaryDetail>? ViewAllSalaryDetails()
        {
            try
            {
                if (_context.SalaryDetails != null)
                {
                    return _context.SalaryDetails.ToList();
                }
                return null;
            }
            catch (Exception ex)
            {
                log.Error($"An error occurred while fetching salary details: {ex.Message}");
                throw new Exception($"An error occurred while fetching salary details: {ex.Message}");
            }
        }
    }
}
