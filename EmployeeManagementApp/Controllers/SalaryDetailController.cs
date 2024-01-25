using EmployeeManagementApp.DAL.Implementations;
using FinalProject.Models;
using log4net;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.Dal.Implementations;

namespace EmployeeManagementApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalaryDetailController : ControllerBase
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(SalaryDetailController));
        private readonly IDEmployee _dEmployee;
        private readonly IDSalaryDetail _dSalaryDetail;


        public SalaryDetailController(IDSalaryDetail dSalaryDetail, IDEmployee dEmployee)
        {
            _dSalaryDetail = dSalaryDetail;
            _dEmployee = dEmployee;
        }
        [HttpPost("createSalaryDetail")]
        public IActionResult CreateSalaryDetail([FromBody] SalaryDetailModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (Guid.TryParse(model.EmployeeId, out Guid employeeId) &&
                    decimal.TryParse(model.BasicSalary, out decimal basicSalary) &&
                    decimal.TryParse(model.Bonuses, out decimal bonuses) &&
                    decimal.TryParse(model.Deductions, out decimal deductions))
                {
                    var newSalaryDetail = new SalaryDetail
                    {
                        EmployeeID = employeeId,
                        BasicSalary = basicSalary,
                        Bonuses = bonuses,
                        Deductions = deductions
                    };

                    _dSalaryDetail.CreateSalaryDetail(newSalaryDetail);
                    log.Info("Salary details added successfully.");
                    return Ok("Salary details added successfully.");
                }
                else
                {
                    log.Error("Invalid input format.");
                    return BadRequest("Invalid input format.");
                }
            }
            catch (Exception ex)
            {
                log.Error($"An error occurred while creating salary details: {ex.Message}");
                return StatusCode(500, $"An error occurred while creating salary details: {ex.Message}");
            }
        }
        [HttpDelete("deleteSalaryDetail/{salaryId}")]
        public IActionResult DeleteSalaryDetail(Guid salaryId)
        {
            try
            {
                var existingSalaryDetail = _dSalaryDetail.GetSalaryDetailById(salaryId);

                if (existingSalaryDetail != null)
                {
                    bool isSuccess = _dSalaryDetail.DeleteSalaryDetail(salaryId);

                    if (isSuccess)
                    {
                        log.Info("Salary details deleted successfully.");
                        return Ok("Salary details deleted successfully.");
                    }
                    else
                    {
                        log.Error("Failed to delete salary details.");
                        return StatusCode(500, "Failed to delete salary details.");
                    }
                }
                else
                {
                    log.Error("Salary details not found.");
                    return NotFound("Salary details not found.");
                }
            }
            catch (Exception ex)
            {
                log.Error($"An error occurred while deleting salary details: {ex.Message}");
                return StatusCode(500, $"An error occurred while deleting salary details: {ex.Message}");
            }
        }
        [HttpPut("updateSalaryDetail/{salaryId}")]
        public IActionResult UpdateSalaryDetail(Guid salaryId, [FromBody] SalaryDetailUpdateModel model)
        {
            try
            {
                var existingSalaryDetail = _dSalaryDetail.GetSalaryDetailById(salaryId);

                if (existingSalaryDetail != null)
                {
                    existingSalaryDetail.BasicSalary = model.BasicSalary;
                    existingSalaryDetail.Bonuses = model.Bonuses;
                    existingSalaryDetail.Deductions = model.Deductions;

                    bool ? isSuccess = _dSalaryDetail.UpdateSalaryDetail(existingSalaryDetail);

                    if (isSuccess==true)
                    {
                        log.Info("Salary details updated successfully.");
                        return Ok("Salary details updated successfully.");
                    }
                    else
                    {

                        log.Error("Failed to update salary details.");
                        return StatusCode(500, "Failed to update salary details.");
                    }
                }
                else
                {
                    log.Error("Salary details not found.");
                    return NotFound("Salary details not found.");
                }
            }
            catch (Exception ex)
            {
                log.Error($"An error occurred while updating salary details: {ex.Message}");
                return StatusCode(500, $"An error occurred while updating salary details: {ex.Message}");
            }
        }
        [HttpGet("viewAllSalaryDetails")]
        public IActionResult ViewAllSalaryDetails()
        {
            try
            {
                var salaryDetails = _dSalaryDetail.ViewAllSalaryDetails();

                if (salaryDetails != null && salaryDetails.Any())
                {
                    log.Info("Salary Details: ");
                    return Ok(salaryDetails);
                }
                else
                {
                    log.Error("No salary details found.");
                    return NotFound("No salary details found.");
                }
            }
            catch (Exception ex)
            {
                log.Error($"An error occurred while fetching salary details: {ex.Message}");
                return StatusCode(500, $"An error occurred while fetching salary details: {ex.Message}");
            }
        }
        [HttpGet("view/salary/{managerId}")]
        public IActionResult GetSalaryDetailsByEmployeeId(string managerId)
        {
            try
            {
                if (Guid.TryParse(managerId, out Guid parsedManagerId))
                {
                    var salaryDetails = _dSalaryDetail.GetSalaryDetailsByEmployeeId(parsedManagerId);

                    if (salaryDetails != null && salaryDetails.Any())
                    {
                        log.Info("Salary Details for this employee: ");
                        return Ok(salaryDetails);
                    }
                    else
                    {
                        log.Error("No salary details found for this employee.");
                        return NotFound("No salary details found for this employee.");
                    }
                }
                else
                {
                    log.Error("Invalid employee ID.");
                    return BadRequest("Invalid employee ID.");
                }
            }
            catch (Exception ex)
            {
                log.Error($"An error occurred while fetching salary details: {ex.Message}");
                return StatusCode(500, $"An error occurred while fetching salary details: {ex.Message}");
            }
        }
        [HttpGet("view/salary/department/{managerId}")]
        public IActionResult ViewAllSalaryDetailsManagerDepartment(string managerId)
        {
            try
            {
                if (Guid.TryParse(managerId, out Guid parsedManagerId))
                {
                    var manager = _dEmployee.GetEmployeeById(parsedManagerId);

                    if (manager != null && manager.Role == "manager")
                    {
                        var salaryDetailsInManagerDepartment = _dSalaryDetail.GetSalaryDetailsByDepartmentId(manager.DepartmentID);

                        if (salaryDetailsInManagerDepartment != null && salaryDetailsInManagerDepartment.Any())
                        {
                            log.Info($"All Salary Detail for This Department: ");
                            return Ok(salaryDetailsInManagerDepartment);
                        }
                        else
                        {
                            log.Error("No salary details found in the manager's department.");
                            return NotFound("No salary details found in the manager's department.");
                        }
                    }
                    else
                    {
                        log.Error("The provided ID does not belong to a manager.");
                        return BadRequest("The provided ID does not belong to a manager.");
                    }
                }
                else
                {
                    log.Error("Invalid manager ID.");
                    return BadRequest("Invalid manager ID.");
                }
            }
            catch (Exception ex)
            {
                log.Error($"An error occurred while fetching salary details: {ex.Message}");
                return StatusCode(500, $"An error occurred while fetching salary details: {ex.Message}");
            }
        }





    }
    public class SalaryDetailModel
    {
        public string ? EmployeeId { get; set; }
        public string ? BasicSalary { get; set; }
        public string ? Bonuses { get; set; }
        public string ? Deductions { get; set; }
    }
    public class SalaryDetailUpdateModel
    {
        public decimal BasicSalary { get; set; }
        public decimal Bonuses { get; set; }
        public decimal Deductions { get; set; }
    }

}
