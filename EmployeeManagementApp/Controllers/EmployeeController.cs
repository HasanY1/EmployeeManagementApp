using FinalProject.Models;
using Microsoft.AspNetCore.Mvc;
using log4net;
using WebApplication2.Dal.Implementations;

namespace EmployeeManagementApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(EmployeeController));
        private readonly IDEmployee _dEmployee;

        public EmployeeController(IDEmployee dEmployee)
        {
            _dEmployee = dEmployee;
        }

        [HttpPost("create")]
        public IActionResult CreateEmployee([FromBody] EmployeeCreateRequestModel model)
        {
            try
            {
                var newEmployee = new Employee
                {
                    Name = model.Name,
                    ContactInfo = model.ContactInfo,
                    Role = model.Role,
                    DepartmentID = model.DepartmentID
                };
                bool? isCreated = _dEmployee.CreateEmployee(newEmployee);

                if (isCreated == true)
                {
                    log.Info("New employee created successfully.");
                    return Ok("New employee created successfully.");
                }
                else if (isCreated == false)
                {
                    log.Error("Failed to create employee.");
                    return StatusCode(500, "Failed to create employee.");
                }
                else
                {
                    log.Error("Invalid input or error occurred while creating employee.");
                    return BadRequest("Invalid input or error occurred while creating employee.");
                }
            }
            catch (Exception ex)
            {
                log.Error($"An error occurred: {ex.Message}");
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpDelete("delete/{employeeId}")]
        public IActionResult DeleteEmployee(Guid employeeId)
        {
            try
            {
                bool? isSuccess = _dEmployee.DeleteEmployee(employeeId);

                if (isSuccess == true)
                {
                    log.Info("Employee deleted successfully.");
                    return Ok("Employee deleted successfully.");
                }
                else
                {
                    log.Warn("Employee not found.");
                    return NotFound("Employee not found.");
                }
            }
            catch (Exception ex)
            {
                log.Error($"An error occurred: {ex.Message}");
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPut("update/{employeeId}")]
        public IActionResult UpdateEmployee(Guid employeeId, Employee updatedEmployee)
        {
            try
            {
                var existingEmployee = _dEmployee.GetEmployeeById(employeeId);

                if (existingEmployee != null)
                {
                    existingEmployee.Name = updatedEmployee.Name;
                    existingEmployee.ContactInfo = updatedEmployee.ContactInfo;
                    existingEmployee.Role = updatedEmployee.Role;
                    existingEmployee.DepartmentID = updatedEmployee.DepartmentID;

                    bool? isUpdated = _dEmployee.UpdateEmployee(existingEmployee);

                    if (isUpdated == true)
                    {
                        log.Info("Employee updated successfully.");
                        return Ok("Employee updated successfully.");
                    }
                    else if (isUpdated == false)
                    {
                        log.Error("Failed to update employee.");
                        return StatusCode(500, "Failed to update employee.");
                    }
                    else
                    {
                        log.Error("Invalid input or error occurred while updating employee.");
                        return BadRequest("Invalid input or error occurred while updating employee.");
                    }
                }
                else
                {
                    log.Warn("Employee not found.");
                    return NotFound("Employee not found.");
                }
            }
            catch (Exception ex)
            {
                log.Error($"An error occurred: {ex.Message}");
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("view/all")]
        public IActionResult ViewAllEmployees()
        {
            try
            {
                List<Employee>? employees = _dEmployee.ViewAllEmployees();

                if (employees != null)
                {
                    log.Info("Retrieved all employees.");
                    return Ok(employees);
                }
                else
                {
                    log.Warn("No employees found.");
                    return NotFound("No employees found.");
                }
            }
            catch (Exception ex)
            {
                log.Error($"An error occurred: {ex.Message}");
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
        [HttpGet("view/{managerId}/employee/{employeeId}")]
        public IActionResult ViewEmployeeById(string managerId, string employeeId)
        {
            try
            {
                if (!Guid.TryParse(managerId, out Guid parsedManagerId) || !Guid.TryParse(employeeId, out Guid parsedEmployeeId))
                {
                    return BadRequest("Invalid IDs provided.");
                }

                var manager = _dEmployee.GetEmployeeById(parsedManagerId);

                if (manager != null && manager.Role == "manager")
                {
                    var employeeToView = _dEmployee.GetEmployeeById(parsedEmployeeId);

                    if (employeeToView != null && employeeToView.DepartmentID == manager.DepartmentID)
                    {
                        var employeeDetails = new
                        {
                            ID = employeeToView.EmployeeID,
                            employeeToView.Name,
                            employeeToView.Role
                        };
                        log.Info("Employee with ID: ");
                        return Ok(employeeDetails);
                    }
                    else
                    {
                        log.Error("Employee not found or not in the same department as the manager.");
                        return NotFound("Employee not found or not in the same department as the manager.");
                    }
                }
                else
                {
                    log.Error("The provided ID does not belong to a manager.");
                    return BadRequest("The provided ID does not belong to a manager.");
                }
            }
            catch (Exception ex)
            {
                log.Error($"An error occurred while fetching employee details: {ex.Message}");
                return StatusCode(500, $"An error occurred while fetching employee details: {ex.Message}");
            }
        }
        [HttpGet("view/{managerId}/employees")]
        public IActionResult ViewAllEmployeesInDepartment(string managerId)
        {
            try
            {
                if (!Guid.TryParse(managerId, out Guid parsedManagerId))
                {
                    log.Error("Invalid manager ID.");
                    return BadRequest("Invalid manager ID.");
                }

                var manager = _dEmployee.GetEmployeeById(parsedManagerId);

                if (manager != null && manager.Role == "manager")
                {
                    var employeesInManagerDepartment = _dEmployee.GetEmployeesByDepartmentId(manager.DepartmentID);

                    if (employeesInManagerDepartment != null && employeesInManagerDepartment.Any())
                    {
                        var employees = employeesInManagerDepartment.Select(employee => new
                        {
                            ID = employee.EmployeeID,
                            employee.Name,
                            employee.Role
                        }).ToList();
                        log.Info("Employees in Department");
                        return Ok(employees);
                    }
                    else
                    {
                        log.Error("No employees found in the manager's department.");
                        return NotFound("No employees found in the manager's department.");
                    }
                }
                else
                {

                    log.Error("The provided ID does not belong to a manager.");
                    return BadRequest("The provided ID does not belong to a manager.");
                }
            }
            catch (Exception ex)
            {
                log.Error($"An error occurred while fetching employees: {ex.Message}");
                return StatusCode(500, $"An error occurred while fetching employees: {ex.Message}");
            }
        }


    }
    public class EmployeeCreateRequestModel
    {
        public string ? Name { get; set; }

        public string ? ContactInfo { get; set; }
        public string ? Role { get; set; }
        public Guid DepartmentID { get; set; }
    }
}
