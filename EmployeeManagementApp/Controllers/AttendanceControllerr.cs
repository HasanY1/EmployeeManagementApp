using EmployeeManagementApp.DAL.Implementations;
using FinalProject.Models;
using Microsoft.AspNetCore.Mvc;
using log4net;
using WebApplication2.Dal.Implementations;

namespace EmployeeManagementApp.Controllers
{
    [Route("api/attendance")]
    [ApiController]
    public class AttendanceController : ControllerBase
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(AttendanceController));

        private readonly IDAttendance _dAttendance;
        private readonly IDEmployee _dEmployee;

        public AttendanceController(IDAttendance dAttendance, IDEmployee dEmployee)
        {
            _dAttendance = dAttendance;
            _dEmployee = dEmployee;
        }

        [HttpPost("mark")]
        public async Task <ActionResult<string>> MarkAttendance([FromBody] AttendanceMarkRequestModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    log.Warn("Invalid model state while marking attendance.");
                    return BadRequest(ModelState);
                }

                var newAttendance = new Attendance
                {
                    EmployeeID = model.EmployeeID,
                    AttendanceDate = model.AttendanceDate,
                    TimeIn = model.TimeIn,
                    TimeOut = model.TimeOut
                };

                bool? isSuccess = await _dAttendance.MarkAttendance(newAttendance);

                if (isSuccess == true)
                {
                    log.Info("Attendance marked successfully.");
                    return Ok("Attendance marked successfully.");
                }

                log.Error("Failed to mark attendance.");
                return StatusCode(500, "Failed to mark attendance.");
            }
            catch (Exception ex)
            {
                log.Error($"An error occurred while marking attendance: {ex.Message}");
                return StatusCode(500, $"An error occurred while marking attendance: {ex.Message}");
            }
        }

        [HttpPut("update/{attendanceId}")]
        public IActionResult UpdateAttendance(Guid attendanceId, [FromBody] AttendanceUpdateRequestModel model)
        {
            try
            {
                var existingAttendance = _dAttendance.GetAttendanceById(attendanceId);

                if (existingAttendance == null)
                {
                    log.Warn("Attendance not found.");
                    return NotFound("Attendance not found.");
                }

                existingAttendance.AttendanceDate = model.AttendanceDate;
                existingAttendance.TimeIn = model.TimeIn;
                existingAttendance.TimeOut = model.TimeOut;

                bool? isSuccess = _dAttendance.UpdateAttendance(existingAttendance);

                if (isSuccess == true)
                {
                    log.Info("Attendance updated successfully.");
                    return Ok("Attendance updated successfully.");
                }

                log.Error("Failed to update attendance.");
                return StatusCode(500, "Failed to update attendance.");
            }
            catch (Exception ex)
            {
                log.Error($"An error occurred while updating attendance: {ex.Message}");
                return StatusCode(500, $"An error occurred while updating attendance: {ex.Message}");
            }
        }



        [HttpDelete("delete/{attendanceId}")]
        public IActionResult DeleteAttendance(Guid attendanceId)
        {
            try
            {
                var existingAttendance = _dAttendance.GetAttendanceById(attendanceId);

                if (existingAttendance == null)
                {
                    log.Warn("Attendance not found.");
                    return NotFound("Attendance not found.");
                }

                bool? isSuccess = _dAttendance.DeleteAttendance(attendanceId);

                if (isSuccess == true)
                {
                    log.Info("Attendance deleted successfully.");
                    return Ok("Attendance deleted successfully.");
                }

                log.Error("Failed to delete attendance.");
                return StatusCode(500, "Failed to delete attendance.");
            }
            catch (Exception ex)
            {
                log.Error($"An error occurred while deleting attendance: {ex.Message}");
                return StatusCode(500, $"An error occurred while deleting attendance: {ex.Message}");
            }
        }


        [HttpGet("view/{employeeId}")]
        public IActionResult ViewAttendanceByEmployeeId(string employeeId)
        {
            try
            {
                if (Guid.TryParse(employeeId, out Guid parsedEmployeeId))
                {
                    var attendanceRecords = _dAttendance.GetAttendanceByEmployeeId(parsedEmployeeId);

                    if (attendanceRecords != null && attendanceRecords.Any())
                    {
                        log.Info("Attendance records retrieved successfully.");
                        return Ok(attendanceRecords);
                    }
                    else
                    {
                        log.Warn("No attendance records found for this employee.");
                        return NotFound("No attendance records found for this employee.");
                    }
                }
                else
                {
                    log.Error("Invalid employee ID provided.");
                    return BadRequest("Invalid employee ID.");
                }
            }
            catch (Exception ex)
            {
                log.Error($"An error occurred: {ex.Message}");
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }


        [HttpGet("view/manager/{managerId}")]
        public IActionResult ViewAttendanceByManagerId(string managerId)
        {
            try
            {
                if (Guid.TryParse(managerId, out Guid parsedManagerId))
                {
                    var manager = _dEmployee.GetEmployeeById(parsedManagerId);

                    if (manager != null && manager.Role == "manager")
                    {
                        var employeesInManagerDepartment = _dEmployee.GetEmployeesByDepartmentId(manager.DepartmentID);

                        if (employeesInManagerDepartment != null && employeesInManagerDepartment.Any())
                        {
                            var employeesAttendance = new Dictionary<string, List<Attendance>>();

                            foreach (var employee in employeesInManagerDepartment)
                            {
                                var attendanceRecords = _dAttendance.GetAttendanceByEmployeeId(employee.EmployeeID);
                                if (attendanceRecords != null && attendanceRecords.Any())
                                {
                                    employeesAttendance.Add(employee.EmployeeID.ToString(), attendanceRecords);
                                }
                            }

                            log.Info("Attendance records retrieved successfully for employees in the manager's department.");
                            return Ok(employeesAttendance);
                        }
                        else
                        {
                            log.Warn("No employees found in the manager's department.");
                            return NotFound("No employees found in the manager's department.");
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
                    log.Error("Invalid manager ID provided.");
                    return BadRequest("Invalid manager ID.");
                }
            }
            catch (Exception ex)
            {
                log.Error($"An error occurred: {ex.Message}");
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("view/all")]
        public IActionResult ViewAllAttendance()
        {
            try
            {
                List<Attendance>? attendances = _dAttendance.ViewAllAttendance();

                if (attendances != null && attendances.Count > 0)
                {
                    log.Info("Retrieved all attendance records.");
                    return Ok(attendances);
                }
                else
                {
                    log.Warn("No attendance records found.");
                    return NotFound("No attendance records found.");
                }
            }
            catch (Exception ex)
            {
                log.Error($"An error occurred: {ex.Message}");
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

    }

    public class AttendanceMarkRequestModel
    {
        public Guid EmployeeID { get; set; }
        public DateTime AttendanceDate { get; set; }
        public TimeSpan TimeIn { get; set; }
        public TimeSpan TimeOut { get; set; }
    }
    public class AttendanceUpdateRequestModel
    {
        public DateTime AttendanceDate { get; set; }
        public TimeSpan TimeIn { get; set; }
        public TimeSpan TimeOut { get; set; }
    }
}
