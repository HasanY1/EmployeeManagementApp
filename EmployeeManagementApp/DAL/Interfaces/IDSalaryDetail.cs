using FinalProject.Models;

namespace EmployeeManagementApp.DAL.Implementations
{
    public interface IDSalaryDetail
    {
        public bool? CreateSalaryDetail(SalaryDetail? newSalaryDetail);
        public List<SalaryDetail>? GetSalaryDetailsByDepartmentId(Guid? departmentId);
        public List<SalaryDetail>? GetSalaryDetailsByEmployeeId(Guid? employeeId);
        public bool? UpdateSalaryDetail(SalaryDetail? updatedSalaryDetail);
        public SalaryDetail? GetSalaryDetailById(Guid salaryId);
        public bool DeleteSalaryDetail(Guid salaryId);
        public List<SalaryDetail>? ViewAllSalaryDetails();
    }
}