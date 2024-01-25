using EmployeeManagementApp.DAL.Implementations;
using FinalProject.Models;
using log4net;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(DepartmentController));
        private readonly IDDepartmnets _dDepartments;

        public DepartmentController(IDDepartmnets dDepartments)
        {
            _dDepartments = dDepartments;
        }

        [HttpPost("create")]
        public IActionResult CreateDepartment([FromBody] DepartmentCreateRequestModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    log.Warn("Invalid model state while creating department.");
                    return BadRequest(ModelState);
                }

                var newDepartment = new Department
                {
                    DepartmentName = model.DepartmentName
                };

                bool? isCreated = _dDepartments.CreateDepartment(newDepartment);

                if (isCreated == true)
                {
                    log.Info("New department created successfully.");
                    return Ok("New department created successfully.");
                }
                else if (isCreated == false)
                {
                    log.Error("Failed to create department.");
                    return StatusCode(500, "Failed to create department.");
                }
                else
                {
                    log.Error("Invalid input or error occurred while creating department.");
                    return BadRequest("Invalid input or error occurred while creating department.");
                }
            }
            catch (Exception ex)
            {
                log.Error($"An error occurred: {ex.Message}");
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPut("update/{departmentId}")]
        public IActionResult UpdateDepartment(Guid departmentId, [FromBody] DepartmentCreateRequestModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    log.Warn("Invalid model state while updating department.");
                    return BadRequest(ModelState);
                }

                var existingDepartment = _dDepartments.GetDepartmentById(departmentId);

                if (existingDepartment != null)
                {
                    existingDepartment.DepartmentName = model.DepartmentName;
                    bool? isUpdated = _dDepartments.UpdateDepartment(existingDepartment);

                    if (isUpdated == true)
                    {
                        log.Info("Department updated successfully.");
                        return Ok("Department updated successfully.");
                    }
                    else if (isUpdated == false)
                    {
                        log.Error("Failed to update department.");
                        return StatusCode(500, "Failed to update department.");
                    }
                    else
                    {
                        log.Error("Invalid input or error occurred while updating department.");
                        return BadRequest("Invalid input or error occurred while updating department.");
                    }
                }
                else
                {
                    log.Warn("Department not found.");
                    return NotFound("Department not found.");
                }
            }
            catch (Exception ex)
            {
                log.Error($"An error occurred: {ex.Message}");
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpDelete("deleteDepartment/{departmentId}")]
        public IActionResult DeleteDepartment(Guid departmentId)
        {
            try
            {
                bool? isSuccess = _dDepartments.DeleteDepartment(departmentId);

                if (isSuccess == true)
                {
                    log.Info("Department deleted successfully.");
                    return Ok("Department deleted successfully.");
                }
                else if (isSuccess == false)
                {
                    log.Error("Failed to delete department.");
                    return StatusCode(500, "Failed to delete department.");
                }
                else
                {
                    log.Error("Failed to delete department. Unknown error.");
                    return StatusCode(500, "Failed to delete department. Unknown error.");
                }
            }
            catch (Exception ex)
            {
                log.Error($"An error occurred: {ex.Message}");
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("viewDepartments")]
        public IActionResult ViewAllDepartments()
        {
            try
            {
                List<Department>? departments = _dDepartments.ViewAllDepartments();

                if (departments != null && departments.Count > 0)
                {
                    log.Info("Retrieved all departments.");
                    return Ok(departments);
                }
                else
                {
                    log.Warn("No departments found.");
                    return NotFound("No departments found.");
                }
            }
            catch (Exception ex)
            {
                log.Error($"An error occurred: {ex.Message}");
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }

    public class DepartmentCreateRequestModel
    {
        public string? DepartmentName { get; set; }
    }
}
